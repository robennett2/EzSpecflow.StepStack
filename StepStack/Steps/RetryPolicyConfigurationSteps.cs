using EzSpecflow.Abstractions;
using TechTalk.SpecFlow;

namespace EzSpecflow.Steps;

[Binding]
public class RetryPolicyConfigurationSteps
{
    private readonly IRetryPolicyFactoryResolver _retryPolicyFactoryResolver;

    internal RetryPolicyConfigurationSteps(IRetryPolicyFactoryResolver retryPolicyFactoryResolver)
    {
        _retryPolicyFactoryResolver = retryPolicyFactoryResolver;
    }
    
    [Given("I am using the (.+) retry policy")]
    public void GivenIAmUsingTheSpecifiedRetryPolicy(string policyName)
    {
        _retryPolicyFactoryResolver.Select(policyName);
    }
    
    [Given("I want to use the default retry policy")]
    public void GivenIWantToUseTheDefaultRetryPolicy()
    {
        _retryPolicyFactoryResolver.UseDefault();
    }
}