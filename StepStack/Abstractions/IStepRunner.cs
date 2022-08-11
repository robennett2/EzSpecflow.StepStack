using System.Threading;
using System.Threading.Tasks;
using StepStack.Models;

namespace StepStack.Abstractions;

public interface IStepRunner
{
    Task Add(IStep step);
    
    Task Execute(CancellationToken cancellationToken = default);
}