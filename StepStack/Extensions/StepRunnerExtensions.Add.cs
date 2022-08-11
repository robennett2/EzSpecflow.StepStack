using System;
using System.Threading.Tasks;
using EzSpecflow.Abstractions;
using EzSpecflow.Models;

namespace EzSpecflow.Extensions;

public static partial class StepRunnerExtensions
{
    public static Task Add(
        this IStepRunner stepRunner,
        string stepName,
        Func<Task> step) =>
        stepRunner.Add(stepName,
            RetryPolicy.None,
            null,
            step);

    public static Task Add(
        this IStepRunner stepRunner,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step) =>
            stepRunner.Add(stepName,
                retryPolicy,
                null,
                step);

    public static async Task Add(
        this IStepRunner stepRunner,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step) => 
            await stepRunner.Add(new SimpleStep(
                stepName,
                step,
                retryPolicy,
                stepDescription
            ));
}