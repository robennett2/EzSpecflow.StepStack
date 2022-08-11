using Polly.Retry;

namespace StepStack.Abstractions;

public interface IRetryPolicyFactory
{
    AsyncRetryPolicy BuildStepPolicy();
    AsyncRetryPolicy BuildStackPolicy();
    AsyncRetryPolicy BuildFramePolicy();
}