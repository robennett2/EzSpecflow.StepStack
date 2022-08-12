using System;
using System.Threading.Tasks;
using EzSpecflow.Abstractions;
using EzSpecflow.Models;

namespace EzSpecflow.Extensions;

public static partial class StepStackExtensions
{
    public static Task Add(
        this IStepStack stepStack,
        string stepName,
        Func<Task> step) =>
            stepStack.Add(stepName,
                RetryPolicy.None,
                null,
                step);

    public static Task Add(
        this IStepStack stepStack,
        string stepName,
        RetryPolicy retryPolicy,
        Func<Task> step) =>
            stepStack.Add(stepName,
                    retryPolicy,
                    null,
                    step);

    public static async Task Add(
        this IStepStack stepStack,
        string stepName,
        RetryPolicy retryPolicy,
        string? stepDescription,
        Func<Task> step)
    {
        IFrame frame = stepStack.CurrentFrame;
        
        await frame.Add(new Step(
            stepName,
            step,
            retryPolicy,
            stepDescription
        ));
    }
}