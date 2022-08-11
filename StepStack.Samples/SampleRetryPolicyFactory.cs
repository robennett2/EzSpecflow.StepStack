using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace EzSpecflow;

public class SampleRetryPolicyFactory : IRetryPolicyFactory
{
    public AsyncRetryPolicy BuildStepPolicy() => 
        Policy
            .Handle<StepRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5,
                fastFirst: true));

    public AsyncRetryPolicy BuildStackPolicy() => BuildStepPolicy();

    public AsyncRetryPolicy BuildFramePolicy() => BuildStepPolicy();
}