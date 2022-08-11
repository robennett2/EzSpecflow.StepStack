using System;
using System.Threading.Tasks;
using StepStack.Abstractions;
using StepStack.Models;

namespace StepStack;

public class SimpleStep : IStep
{
    private readonly Func<Task> _step;

    public SimpleStep(
        string stepName, 
        Func<Task> step, 
        RetryPolicy retryPolicy, 
        string? stepDescription = null)
    {
        _step = step;
        StepName = stepName;
        RetryPolicy = retryPolicy;
        StepDescription = stepDescription;
    }

    public int ExecutionCount { get; private set; }
    public RetryPolicy RetryPolicy { get; }
    public string StepName { get; }
    public string? StepDescription { get; }
    public virtual async Task<StepResult> ExecuteStep()
    {
        ExecutionCount++;
        
        try
        {
            await _step.Invoke();
            return new StepResult(this, true, ExecutionCount);
        }
        catch (Exception ex)
        {
            return new StepResult(this, false, ExecutionCount, null, ex);
        }
    }
}