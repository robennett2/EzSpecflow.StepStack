using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Extensions;
using TechTalk.SpecFlow;

namespace EzSpecflow;

[Binding]
public sealed class Hooks
{
    private readonly IObjectContainer _objectContainer;

    public Hooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario(Order = 1)]
    public void BeforeScenario()
    {
        _objectContainer.RegisterTypeAs<RetryPolicyFactoryResolver, IRetryPolicyFactoryResolver>();
        _objectContainer.RegisterStepRunner<Frame>();
        _objectContainer.RegisterRetryPolicyFactory<DefaultRetryPolicyFactory>("default");
    }
}