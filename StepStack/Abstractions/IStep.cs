using System.Threading;
using System.Threading.Tasks;
using EzSpecflow.Models;

namespace EzSpecflow.Abstractions;

public interface IStep : IExecutable<StepResult>
{
    public RetryPolicy RetryPolicy { get; }
}