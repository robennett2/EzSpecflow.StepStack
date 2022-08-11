using System.Threading;
using System.Threading.Tasks;

namespace EzSpecflow.Abstractions;

public interface IStepRunner
{
    Task Add(IStep step);
    
    Task Execute(CancellationToken cancellationToken = default);
}