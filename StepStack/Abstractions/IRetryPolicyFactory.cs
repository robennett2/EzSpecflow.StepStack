using Polly.Retry;

namespace EzSpecflow.Abstractions;

public interface IRetryPolicyFactory
{
    AsyncRetryPolicy BuildStepPolicy();
    AsyncRetryPolicy BuildStackPolicy();
    AsyncRetryPolicy BuildFramePolicy();
}