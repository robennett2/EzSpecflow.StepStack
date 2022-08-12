using System;
using System.Diagnostics;
using System.Linq;
using BoDi;
using EzSpecflow.Abstractions;
using EzSpecflow.Extensions;

namespace EzSpecflow;

internal class RetryPolicyFactoryResolver : IRetryPolicyFactoryResolver
{
    private readonly IObjectContainer _objectContainer;

    public RetryPolicyFactoryResolver(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }
    public string? CurrentFactoryName { get; private set; }

    public void UseDefault()
    {
        CurrentFactoryName = null;
    }

    public void Select(string? factoryName)
    {
        if (factoryName is not null && _objectContainer.IsRegistered<IRetryPolicyFactory>(factoryName) is false)
        {
            factoryName = null;
        }
        
        CurrentFactoryName = factoryName;
    }

    public IRetryPolicyFactory Resolve()
    {
        var resolvedFactory = CurrentFactoryName is null
            ? _objectContainer.ResolveAll<IRetryPolicyFactory>().Select(x =>
            {
                Debug.WriteLine($"{x.GetType().FullName}");
                return x;
            }).ToList().First()
            : _objectContainer.ResolveRetryPolicyFactory(CurrentFactoryName);
        Debug.WriteLine($"Resolved Factory: {resolvedFactory.GetType().FullName}");
        return resolvedFactory;
    }
}