using System.Diagnostics;
using EzSpecflow.Abstractions;
using EzSpecflow.Extensions;
using EzSpecflow.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace EzSpecflow.Steps;

[Binding]
public sealed class StackSampleSteps
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly IFrame _frame;

    public StackSampleSteps(ScenarioContext scenarioContext, IFrame frame)
    {
        _scenarioContext = scenarioContext;
        _frame = frame;
    }

    [Given(@"I have a number (\d+)")]
    public Task GivenIHaveANumber(int number)
    {
        Debug.WriteLine($"Given I have a number {number}");
        _scenarioContext[CurrentValueKey] = number;
        return Task.CompletedTask;
    }
    
    [Given(@"I add (\d+)")]
    public async Task GivenIAdd(int toAdd)
    {
        await _frame.Add(
            _scenarioContext.StepContext.StepInfo.Text,
            () => Task.Run(() =>
            {
                Debug.WriteLine($"Given I add {toAdd}");
                _scenarioContext[CurrentValueKey] = _scenarioContext.Get<int>(CurrentValueKey) + toAdd;
            }));
    }    
    
    [Given(@"I add (\d+) until (\d+)")]
    public async Task GivenIAddUntil(int toAdd, int until)
    {
        await _frame.Add(
            _scenarioContext.StepContext.StepInfo.Text,
            RetryPolicy.Step,
            () => Task.Run(() =>
            {
                Debug.WriteLine($"Given I add {toAdd} until {until}");
                _scenarioContext[CurrentValueKey] = _scenarioContext.Get<int>(CurrentValueKey) + toAdd;

                if (_scenarioContext.Get<int>(CurrentValueKey) < until)
                {
                    throw new AssertFailedException($"Need to add {toAdd} until {until}");
                }
            }));
    }
    
    [Given(@"I subtract (\d+)")]
    public async Task GivenISubtract(int toSubtract)
    {
        await _frame.Add(
            _scenarioContext.StepContext.StepInfo.Text,
            () => Task.Run(() =>
            {
                Debug.WriteLine($"Given I subtract {toSubtract}");
                _scenarioContext[CurrentValueKey] = _scenarioContext.Get<int>(CurrentValueKey) - toSubtract;
            }));
    }
    
    [Then(@"I throw if less than (\d+) without retry")]
    public async Task ThenIThrowIfLessThenWithoutRetry(int ceiling)
    {
        await _frame.Execute(
            _scenarioContext.StepContext.StepInfo.Text,
            () => Task.Run(() =>
            {
                int value = _scenarioContext.Get<int>(CurrentValueKey);
                Debug.WriteLine($"Then I throw if {value} less than {ceiling} without retry");
                if (value < ceiling)
                {
                    throw new InvalidOperationException($"{value} is less than {ceiling}");
                }
            }));
    }
    
    [Then(@"I throw if less than (\d+) with stack retry")]
    public async Task ThenIThrowIfLessThenWithStackRetry(int ceiling)
    {
        await _frame.Execute(
            _scenarioContext.StepContext.StepInfo.Text,
            RetryPolicy.Stack,
            () => Task.Run(() =>
            {
                int value = _scenarioContext.Get<int>(CurrentValueKey);
                Debug.WriteLine($"Then I throw if {value} less than {ceiling} with stack retry");
                if (value < ceiling)
                {
                    throw new InvalidOperationException($"{value} is less than {ceiling}");
                }
            }));
    }

    public const string CurrentValueKey = $"{nameof(StackSampleSteps)}_CurrentValue";
}