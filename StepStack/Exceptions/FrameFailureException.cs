using System;

namespace EzSpecflow.Exceptions;

public class FrameFailureException : Exception
{
    public FrameFailureException(string? message, Exception? exception) : base(message, exception)
    {
        
    }
}