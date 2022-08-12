using System;
using EzSpecflow.Abstractions;

namespace EzSpecflow.Models;

public record StepResult(
    IStep Step,
    bool Success,
    int ExecutionCount,
    string? Message = null,
    Exception? Exception = null)
{
    public override string ToString() => Success switch
    {
        true => $"[{Step.Name}] was successful on the {ExecutionCount} execution",
        false => $"[{Step.Name}] failed with {ErrorMessage} on the {ExecutionCount} execution",
    };

    private string? ErrorMessage => Success ? null : Message ?? Exception?.Message ?? "no message";
}

public record FrameResult(
    IFrame Frame,
    bool Success,
    int ExecutionCount,
    string? Message = null,
    Exception? Exception = null)
{
    public override string ToString() => Success switch
    {
        true => $"[{Frame.Name}] was successful on the {ExecutionCount} execution",
        false => $"[{Frame.Name}] failed with {ErrorMessage} on the {ExecutionCount} execution",
    };

    private string? ErrorMessage => Success ? null : Message ?? Exception?.Message ?? "no message";
}