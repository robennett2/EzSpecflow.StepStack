using System;
using EzSpecflow.Abstractions;
using EzSpecflow.Exceptions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace EzSpecflow;

internal sealed class DefaultRetryPolicyFactory : IRetryPolicyFactory
{
    public AsyncRetryPolicy BuildStepPolicy() =>
        Policy
            .Handle<StepRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 20,
                fastFirst: true));

    public AsyncRetryPolicy BuildStackPolicy() =>
        Policy
            .Handle<StackRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(30),
                retryCount: 50,
                fastFirst: true));

    public AsyncRetryPolicy BuildFramePolicy() =>
        Policy
            .Handle<FrameRetryNeededException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromMinutes(1),
                retryCount: 50,
                fastFirst: true));
}