using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using StepStack.Abstractions;
using StepStack.Exceptions;

namespace StepStack.Samples;

public class SampleRetryPolicyFactory : IRetryPolicyFactory
{
    public AsyncRetryPolicy BuildStepPolicy() =>
        Policy
            .Handle<StepRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5,
                fastFirst: true));

    public AsyncRetryPolicy BuildStackPolicy() =>
        Policy
            .Handle<StackRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5,
                fastFirst: true));

    public AsyncRetryPolicy BuildFramePolicy() =>
        Policy
            .Handle<FrameRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5,
                fastFirst: true));
}