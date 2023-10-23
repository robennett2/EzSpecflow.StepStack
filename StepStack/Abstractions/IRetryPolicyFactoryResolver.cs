namespace EzSpecflow.Abstractions;

internal interface IRetryPolicyFactoryResolver
{
    string CurrentFactoryName { get; }
        void UseDefault();
        void Select(string factoryName);
        IRetryPolicyFactory Resolve();
}