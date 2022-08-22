using System.Threading;
using System.Threading.Tasks;

namespace EzSpecflow.Abstractions;

public interface IExecutable<TExecutionResult> where TExecutionResult : class
{
    public int ExecutionCount { get; }
    public string Name { get; }
    public string? Description { get; }
    public Task<TExecutionResult> Execute(CancellationToken cancellationToken = default);
}