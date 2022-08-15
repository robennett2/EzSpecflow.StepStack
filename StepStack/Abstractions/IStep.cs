using System.Threading;
using System.Threading.Tasks;
using EzSpecflow.Models;

namespace EzSpecflow.Abstractions;

public interface IStep : IExecutable<StepResult>
{
    public RetryPolicy RetryPolicy { get; }
}

public interface IExecutable<TExecutionResult> where TExecutionResult : class
{
    public int ExecutionCount { get; }
    public string Name { get; }
    public string? Description { get; }
    public Task<TExecutionResult> Execute(CancellationToken cancellationToken = default);
}