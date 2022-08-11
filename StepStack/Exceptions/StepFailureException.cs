using System;

namespace EzSpecflow.Exceptions;

public class StepFailureException : Exception
{
    public StepFailureException(string? message, Exception? exception) : base(message, exception)
    {
        
    }
}