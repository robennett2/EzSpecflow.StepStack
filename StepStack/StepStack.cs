using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using Microsoft.Extensions.Logging;

namespace EzSpecflow;

internal sealed class StepStack : IStepStack
{
    private readonly IObjectContainer _objectContainer;
    private readonly ILogger<StepStack>? _logger;
    private readonly IRetryPolicyFactory _retryPolicyFactory;
    private ConcurrentQueue<IFrame> _frames = new();
    private ConcurrentQueue<IFrame> _executedFrames = new();

    public StepStack(
        IRetryPolicyFactoryResolver retryPolicyFactoryResolver,
        IObjectContainer objectContainer)
    {
        Name = "Step Stack";
        _logger = objectContainer.ResolveAll<ILogger<StepStack>>().FirstOrDefault();
        _retryPolicyFactory = retryPolicyFactoryResolver.Resolve();
        _objectContainer = objectContainer;

        NewFrame(
            new Frame("default",
                _retryPolicyFactory,
                objectContainer));
    }

    public IFrame CurrentFrame { get; private set; } = null!;
    public IReadOnlyList<IFrame> Frames => _frames.ToList();
    public int ExecutionCount { get; private set; }
    public string Name { get; }
    public string? Description { get; }
    
    public void NewFrame(IFrame frame)
    {
        CurrentFrame = frame;
        _frames.Enqueue(frame);
    }

    public void NewFrame(string name)
    {
        NewFrame(
            new Frame(name,
                _retryPolicyFactory,
                _objectContainer));
    }

    public async Task<object> Execute(CancellationToken cancellationToken = default)
    {
        ExecutionCount++;
        _executedFrames = new ConcurrentQueue<IFrame>();
        
        try
        {
            await _retryPolicyFactory
                .BuildStackPolicy()
                .ExecuteAsync(async () =>
                {
                    while (cancellationToken.IsCancellationRequested is false && _frames.IsEmpty is false)
                    {
                        if (_frames.TryDequeue(out var frame))
                        {
                            await frame.Rewind();
                            
                            var result = await frame.Execute(cancellationToken);
                                    _executedFrames.Enqueue(frame);

                            if (result.Success is false)
                            {
                                _logger?.LogError(result.Exception,
                                    result.Message);
                            }
                        }
                    }
                });
        }
        catch (Exception ex)
        {
            throw new StackFailureException(ex.Message, ex);
        }

        return new object();
    }
}