using System;
using System.Threading.Tasks;
using StepStack.Abstractions;
using StepStack.Models;

namespace StepStack.Extensions;

public static partial class StepRunnerExtensions
{
    public static Task Execute(
        this IStepRunner stepRunner,
        string stepName,
        Func<Task> step) =>
        stepRunner.Execute(stepName,
            RetryPolicy.None,
            null,
            step);

    public static Task Execute(
        this IStepRunner stepRunner,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step)
    {
        return stepRunner.Execute(stepName,
            retryPolicy,
            null,
            step);
    }

    public static async Task Execute(
        this IStepRunner stepRunner,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step)
    {
        await stepRunner.Add(
            stepName,
            retryPolicy,
            stepDescription,
            step
        );

        await stepRunner.Execute();
    }
}