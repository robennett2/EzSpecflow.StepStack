using System;
using System.Threading;
using System.Threading.Tasks;
using EzSpecflow.Abstractions;
using EzSpecflow.Models;

namespace EzSpecflow.Extensions;

public static partial class FrameExtensions
{
    public static Task Execute(
        this IFrame frame,
        string stepName,
        Func<Task> step,
        CancellationToken cancellationToken = default) =>
        frame.Execute(stepName,
            RetryPolicy.None,
            null,
            step,
            cancellationToken: cancellationToken);

    public static Task Execute(
        this IFrame frame,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step,
        CancellationToken cancellationToken = default)
    {
        return frame.Execute(stepName,
            retryPolicy,
            null,
            step,
            cancellationToken: cancellationToken);
    }

    public static async Task Execute(
        this IFrame frame,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step,
        CancellationToken cancellationToken = default)
    {
        await frame.Add(
            stepName,
            retryPolicy,
            stepDescription,
            step
        );

        await frame.Execute(cancellationToken);
    }
}