using System;
using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace EzSpecflow;

internal sealed class DefaultRetryPolicyFactory : IRetryPolicyFactory
{
    private AsyncRetryPolicy DefaultPolicy<TException>() where TException : Exception => 
        Policy
            .Handle<TException>()
            .WaitAndRetryAsync(5, retry => TimeSpan.FromSeconds(1 * (retry ^ 2)));

    public AsyncRetryPolicy BuildStepPolicy() => DefaultPolicy<StepRetryNeededException>();
    public AsyncRetryPolicy BuildFramePolicy() => DefaultPolicy<FrameRetryNeededException>();
    public AsyncRetryPolicy BuildStackPolicy() => DefaultPolicy<StackRetryNeededException>();
}