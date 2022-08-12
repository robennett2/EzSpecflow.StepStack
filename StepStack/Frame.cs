using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using EzSpecflow.Models;
using Microsoft.Extensions.Logging;

namespace EzSpecflow;

internal sealed class Frame : IFrame
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

    public async Task<FrameResult> Execute(CancellationToken cancellationToken = default)
    {
        try
        {
            ExecutionCount++;
            await _retryPolicyFactory
                .BuildStackPolicy()
                .ExecuteAsync(async () =>
                {
                    while (cancellationToken.IsCancellationRequested is false && _steps.IsEmpty is false)
                    {
                        if (_steps.TryDequeue(out var step))
                        {
                            await _retryPolicyFactory
                                .BuildStepPolicy()
                                .ExecuteAsync(async () =>
                                {
                                    var result = await step.Execute(cancellationToken);
                                    _executedSteps.Enqueue(step);

                                    if (result.Success is false)
                                    {
                                        _logger?.LogError(result.Exception,
                                            result.Message);

                                        HandleRetryPolicy(result);
                                    }
                                });
                        }
                    }
                });
        }
        catch (Exception ex) when (ex is not FrameRetryNeededException)
        {
            throw new FrameFailureException(ex.Message, ex);
        }

        void HandleRetryPolicy(StepResult result)
        {
            switch (result.Step.RetryPolicy)
            {
                case RetryPolicy.None:
                    throw new StepFailureException(result.Message, result.Exception);
                case RetryPolicy.Step:
                    throw new StepRetryNeededException(result.Exception);
                case RetryPolicy.Stack:
                    Rewind();
                    throw new StackRetryNeededException(result.Exception);
                case RetryPolicy.Frame:
                    throw new FrameRetryNeededException(result.Exception);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return new FrameResult(
            this,
            true,
            ExecutionCount);
    }

    private void Rewind()
    {
        _steps = new ConcurrentQueue<IStep>(_executedSteps.ToList().Concat(_steps));
    }
}