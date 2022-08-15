using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EzSpecflow.Models;

namespace EzSpecflow.Abstractions;

public interface IFrame : IExecutable<FrameResult>
{
    IReadOnlyList<IStep> Steps { get; }
    
    Task Add(IStep step);

    public void Rewind();
}