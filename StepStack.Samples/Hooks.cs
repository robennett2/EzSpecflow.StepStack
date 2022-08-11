using BoDi;
using StepStack.Extensions;
using TechTalk.SpecFlow;

namespace StepStack.Samples;

[Binding]
public sealed class Hooks
{
    private readonly IObjectContainer _objectContainer;

    public Hooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario(Order = 2)]
    public void BeforeScenario()
    {
        _objectContainer.RegisterRetryPolicyFactory<SampleRetryPolicyFactory>();
    }
}