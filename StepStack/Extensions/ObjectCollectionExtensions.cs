using BoDi;
using EzSpecflow.Abstractions;

namespace EzSpecflow.Extensions;

public static class ObjectCollectionExtensions
{
    public static IStrategyRegistration RegisterStepRunner<TStepRunner>(this IObjectContainer objectContainer)
        where TStepRunner : class, IStepRunner =>
        objectContainer.RegisterTypeAs<TStepRunner, IStepRunner>();
    
    public static IStrategyRegistration RegisterRetryPolicyFactory<TRetryPolicyFactory>(this IObjectContainer objectContainer)
        where TRetryPolicyFactory : class, IRetryPolicyFactory =>
        objectContainer.RegisterTypeAs<TRetryPolicyFactory, IRetryPolicyFactory>();
}