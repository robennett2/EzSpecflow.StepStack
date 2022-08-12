using System;
using System.Threading;
using System.Threading.Tasks;
using EzSpecflow.Abstractions;
using EzSpecflow.Models;

namespace EzSpecflow.Extensions;

public static partial class StepStackExtensions
{
    public static Task Execute(
        this IStepStack stepStack,
        string stepName,
        Func<Task> step,
        CancellationToken cancellationToken = default) =>
            stepStack.Execute(stepName,
                RetryPolicy.None,
                null,
                step,
                cancellationToken: cancellationToken);

    public static Task Execute(
        this IStepStack stepStack,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step,
        CancellationToken cancellationToken = default) =>
            stepStack.Execute(stepName,
                retryPolicy,
                null,
                step,
                cancellationToken: cancellationToken);

    public static async Task Execute(
        this IStepStack stepStack,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step,
        CancellationToken cancellationToken = default)
    {
        IFrame frame = stepStack.CurrentFrame;
        
        await frame.Add(
            stepName,
            retryPolicy,
            stepDescription,
            step
        );

        await stepStack.Execute(cancellationToken);
    }
}