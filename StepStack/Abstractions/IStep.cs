using System.Threading.Tasks;
using EzSpecflow.Models;

namespace EzSpecflow.Abstractions;

public interface IStep
{
    public int ExecutionCount { get; }
    public RetryPolicy RetryPolicy { get; }
    public string StepName { get; }
    public string? StepDescription { get; }
    public Task<StepResult> ExecuteStep();
}