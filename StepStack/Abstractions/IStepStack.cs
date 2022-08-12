using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EzSpecflow.Abstractions;

public interface IStepStack : IExecutable<object>
{
    public IFrame CurrentFrame { get; }
    
    public IReadOnlyList<IFrame> Frames { get; }
    
    public void NewFrame(IFrame frame);
    public void NewFrame(string name);
}