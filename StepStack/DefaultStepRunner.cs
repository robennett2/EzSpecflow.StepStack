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

internal sealed class DefaultStepRunner : IStepRunner
{
    private readonly ILogger<DefaultStepRunner>? _logger;
    private readonly IRetryPolicyFactory _retryPolicyFactory;

    public DefaultStepRunner(IObjectContainer objectContainer, IRetryPolicyFactoryResolver retryPolicyFactoryResolver)
    {
        _logger = objectContainer.ResolveAll<ILogger<DefaultStepRunner>>().FirstOrDefault();
        _retryPolicyFactory = retryPolicyFactoryResolver.Resolve();
    }
    
    private ConcurrentQueue<IStep> _steps = new();
    private ConcurrentQueue<IStep> _executedSteps = new();
    public IReadOnlyList<IStep> Steps => _steps.ToList();

    public Task Add(IStep step)
    {
        _steps.Enqueue(step);
        return Task.CompletedTask;
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
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
                                var result = await step.ExecuteStep();
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

        void HandleRetryPolicy(StepResult result)
        {
            switch (result.Step.RetryPolicy)
            {
                default: break;
                case RetryPolicy.None:
                    throw new StepFailureException(result.Message, result.Exception);
                case RetryPolicy.Step:
                    throw new StepRetryNeededException();
                case RetryPolicy.Stack:
                    Rewind();
                    throw new StackRetryNeededException();
                case RetryPolicy.Frame:
                    throw new FrameRetryNeededException();
            }
        }
    }

    private void Rewind()
    {
        _steps = new ConcurrentQueue<IStep>(_executedSteps.ToList().Concat(Steps));
    }
}