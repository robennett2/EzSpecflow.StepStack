using System;

namespace EzSpecflow.Exceptions;

public class StackRetryNeededException : Exception
{
    public StackRetryNeededException(Exception? e) : base(null, e)
    {
        
    }
}