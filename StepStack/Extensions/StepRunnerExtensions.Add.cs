using System;
using System.Threading.Tasks;
using EzSpecflow.Abstractions;
using EzSpecflow.Models;

namespace EzSpecflow.Extensions;

public static partial class FrameExtensions
{
    public static Task Add(
        this IFrame frame,
        string stepName,
        Func<Task> step) =>
        frame.Add(stepName,
            RetryPolicy.None,
            null,
            step);

    public static Task Add(
        this IFrame frame,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step) =>
            frame.Add(stepName,
                retryPolicy,
                null,
                step);

    public static async Task Add(
        this IFrame frame,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step) => 
            await frame.Add(new Step(
                stepName,
                step,
                retryPolicy,
                stepDescription
            ));
}