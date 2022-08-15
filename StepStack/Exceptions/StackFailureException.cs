using System;

namespace EzSpecflow.Exceptions;

public class StackFailureException : Exception
{
    public StackFailureException(string? message, Exception? exception) : base(message, exception)
    {
        
    }
}