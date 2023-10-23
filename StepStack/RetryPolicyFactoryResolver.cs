using System;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Extensions;

namespace EzSpecflow;

internal sealed class RetryPolicyFactoryResolver : IRetryPolicyFactoryResolver
{
    private readonly IObjectContainer _objectContainer;

    public RetryPolicyFactoryResolver(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    public string CurrentFactoryName { get; private set; } = "default";

    public void UseDefault()
    {
        CurrentFactoryName = "default";
    }

    public void Select(string factoryName)
    {
        if (_objectContainer.IsRegistered<IRetryPolicyFactory>(factoryName) is false)
        {
            UseDefault();
            return;
        }

        CurrentFactoryName = factoryName;
    }

    public IRetryPolicyFactory Resolve()
    {
        var resolvedFactory = _objectContainer.ResolveRetryPolicyFactory(CurrentFactoryName);
        Console.WriteLine($"Resolved Retry Policy Factory: {resolvedFactory.GetType().FullName}");
        return resolvedFactory;
    }

    public void SetDefaultFactoryName(string defaultFactoryName)
    {
        DefaultFactoryName = defaultFactoryName;
    }
}