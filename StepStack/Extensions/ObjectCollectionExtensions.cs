using BoDi;
using EzSpecflow.Abstractions;

namespace EzSpecflow.Extensions;

public static class ObjectCollectionExtensions
{
    public static IStrategyRegistration RegisterStepRunner<TStepRunner>(this IObjectContainer objectContainer)
        where TStepRunner : class, IFrame =>
        objectContainer.RegisterTypeAs<TStepRunner, IFrame>();
    
    public static IStrategyRegistration RegisterRetryPolicyFactory<TRetryPolicyFactory>(this IObjectContainer objectContainer, string name)
        where TRetryPolicyFactory : class, IRetryPolicyFactory =>
        objectContainer.RegisterTypeAs<TRetryPolicyFactory, IRetryPolicyFactory>(name.ToLower());
    
    public static IRetryPolicyFactory ResolveRetryPolicyFactory(this IObjectContainer objectContainer, string name = "default") =>
        objectContainer.Resolve<IRetryPolicyFactory>(name.ToLower());
}