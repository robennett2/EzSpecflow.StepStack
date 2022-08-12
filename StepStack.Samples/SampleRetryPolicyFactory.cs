using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace EzSpecflow;

public class SampleRetryPolicyFactory : IRetryPolicyFactory
{
    private AsyncRetryPolicy DefaultPolicy<TException>() where TException : Exception => Policy
        .Handle<TException>()
        .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromSeconds(1),
            retryCount: 5,
            fastFirst: true));
    
    public AsyncRetryPolicy BuildStepPolicy() => DefaultPolicy<StepRetryNeededException>();

    public AsyncRetryPolicy BuildStackPolicy() => DefaultPolicy<StackRetryNeededException>();

    public AsyncRetryPolicy BuildFramePolicy() => DefaultPolicy<FrameRetryNeededException>();
}

public class TestRetryPolicyFactory : IRetryPolicyFactory
{
    public AsyncRetryPolicy BuildStepPolicy() => throw new NotImplementedException();

    public AsyncRetryPolicy BuildStackPolicy() => BuildStepPolicy();

    public AsyncRetryPolicy BuildFramePolicy() => BuildStepPolicy();
}