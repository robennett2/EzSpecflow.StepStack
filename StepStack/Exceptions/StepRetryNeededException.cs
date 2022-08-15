using System;

namespace EzSpecflow.Exceptions;

public class StepRetryNeededException : Exception
{
    public StepRetryNeededException(Exception? e) : base(null, e)
    {
        
    }
}