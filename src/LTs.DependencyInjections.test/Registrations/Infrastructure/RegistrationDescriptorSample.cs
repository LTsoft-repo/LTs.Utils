using LTs.DependencyInjections.Abstractions;

#pragma warning disable IDE0290

namespace LTs.DependencyInjections.test.Registrations.Infrastructure;

internal class RegistrationDescriptorSample : IRegistrationDescriptor
{
    public RegistrationDescriptorSample( Type serviceType, Type implementationType, string? tag )
    {
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Tag = tag;
    }

    public Type ServiceType { get; }

    public Type ImplementationType { get; }

    public string? Tag { get; }
}