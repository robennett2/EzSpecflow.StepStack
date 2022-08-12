using System;

namespace EzSpecflow.Exceptions;

public class FrameRetryNeededException : Exception
{
    public FrameRetryNeededException(Exception? e) : base(null, e)
    {
        
    }
}