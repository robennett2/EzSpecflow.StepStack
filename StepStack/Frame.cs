using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using EzSpecflow.Models;
using Microsoft.Extensions.Logging;

namespace EzSpecflow;

public class Frame : IFrame
{
    private readonly ILogger<Frame>? _logger;
    private readonly IRetryPolicyFactory _retryPolicyFactory;
    private ConcurrentQueue<IStep> _steps = new();
    private ConcurrentQueue<IStep> _executedSteps = new();
    
    public Frame(
        string name, 
        IRetryPolicyFactory retryPolicyFactory, 
        IObjectContainer objectContainer, 
        string? description = null)
    {
        Name = name;
        Description = description;
        _logger = objectContainer.ResolveAll<ILogger<Frame>>().FirstOrDefault();
        _retryPolicyFactory = retryPolicyFactory;
    }

    public IReadOnlyList<IStep> Steps => _steps.ToList();
    public int ExecutionCount { get; private set; }
    public string Name { get; }
    public string? Description { get; }

    public Task Add(IStep step)
    {
        _steps.Enqueue(step);
        return Task.CompletedTask;
    }

    public virtual async Task<FrameResult> Execute(CancellationToken cancellationToken = default)
    {
        ExecutionCount++;
        Debug.WriteLine($"Executing Frame {Name} - Execution {ExecutionCount}");
        
        _executedSteps = new ConcurrentQueue<IStep>();

        try
        {
            await _retryPolicyFactory
                .BuildFramePolicy()
                .ExecuteAsync(async () =>
                {
                    while (cancellationToken.IsCancellationRequested is false && _steps.IsEmpty is false)
                    {
                        if (_steps.TryDequeue(out var step))
                        {
                            Debug.WriteLine($"Dequeue Step {step.Name} Count {_steps.Count}");
                            await _retryPolicyFactory
                                .BuildStepPolicy()
                                .ExecuteAsync(async () =>
                                {
                                    _executedSteps.Enqueue(step);
                                    var result = await step.Execute(cancellationToken);

                                    if (result.Success is false)
                                    {
                                        Debug.WriteLine($"Step {step.Name} failed");
                                        _logger?.LogError(result.Exception,
                                            result.Message);

                                        HandleRetryPolicy(result);
                                    }
                                });
                        }
                    }
                });
        }
        catch (StackRetryNeededException ex)
        {
            return new FrameResult(this, false, ExecutionCount, null, ex);
        }
        catch (Exception ex)
        {
            throw new FrameFailureException(ex.Message, ex);
        }

        FrameResult HandleRetryPolicy(StepResult result)
        {
            Rewind();
            switch (result.Step.RetryPolicy)
            {
                case RetryPolicy.None:
                    throw new StepFailureException(result.Message, result.Exception);
                case RetryPolicy.Step:
                    throw new StepRetryNeededException(result.Exception);
                case RetryPolicy.Frame:
                    throw new FrameRetryNeededException(result.Exception);
                case RetryPolicy.Stack:
                    throw new StackRetryNeededException(result.Exception);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return new FrameResult(
            this,
            true,
            ExecutionCount);
    }

    public void Rewind()
    {
        Debug.WriteLine($"Rewind Stack: Executed {_executedSteps.Count} Pending {_steps.Count}");
        _steps = new ConcurrentQueue<IStep>(_executedSteps.ToList().Concat(_steps));
        _executedSteps.Clear();
        Debug.WriteLine($"Rewound Stack: Executed {_executedSteps.Count} Pending {_steps.Count}");
    }
}