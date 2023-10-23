using System;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Extensions;

namespace EzSpecflow;

internal sealed class RetryPolicyFactoryResolver : IRetryPolicyFactoryResolver
{
    private string? _currencyFactoryName { get; set; }
    private readonly IObjectContainer _objectContainer;

    public RetryPolicyFactoryResolver(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }


    public string CurrentFactoryName
    {
        get
        {
            return _currencyFactoryName ?? DefaultFactoryName;
        }
        private set
        {
            _currencyFactoryName = value;
        }
    }

    public string DefaultFactoryName { get; private set; } = "default";

    public void UseDefault()
    {
        CurrentFactoryName = DefaultFactoryName;
    }

    public void Select(string factoryName)
    {
        if (_objectContainer.IsRegistered<IRetryPolicyFactory>(factoryName) is false)
        {
            UseDefault();
            Console.WriteLine($"No policy factory with name {factoryName} found, using default {CurrentFactoryName}");

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