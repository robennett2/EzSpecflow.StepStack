using BoDi;
using EzSpecflow.Abstractions;

namespace EzSpecflow.Extensions;

public static class ObjectCollectionExtensions
{
    public static IStrategyRegistration RegisterStepStack<TStepStack>(this IObjectContainer objectContainer)
        where TStepStack : class, IStepStack =>
        objectContainer.RegisterTypeAs<TStepStack, IStepStack>();
    
    public static IStrategyRegistration RegisterRetryPolicyFactory<TRetryPolicyFactory>(this IObjectContainer objectContainer, string name)
        where TRetryPolicyFactory : class, IRetryPolicyFactory =>
        objectContainer.RegisterTypeAs<TRetryPolicyFactory, IRetryPolicyFactory>(name.ToLower());

    public static void RegisterRetryPolicyAsDefaultFactory<TRetryPolicyFactory>(this IObjectContainer objectContainer, string name)
        where TRetryPolicyFactory : class, IRetryPolicyFactory
    {
        objectContainer.RegisterTypeAs<TRetryPolicyFactory, IRetryPolicyFactory>(name.ToLower());

        var policyFactory = objectContainer.Resolve<IRetryPolicyFactoryResolver>();
        policyFactory.SetDefaultFactoryName(name);
    }
    
    public static IRetryPolicyFactory ResolveRetryPolicyFactory(this IObjectContainer objectContainer, string name) =>
        objectContainer.Resolve<IRetryPolicyFactory>(name.ToLower());
}