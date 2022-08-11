using System.Threading.Tasks;
using StepStack.Models;

namespace StepStack.Abstractions;

public interface IStep
{
    public int ExecutionCount { get; }
    public RetryPolicy RetryPolicy { get; }
    public string StepName { get; }
    public string? StepDescription { get; }
    public Task<StepResult> ExecuteStep();
}