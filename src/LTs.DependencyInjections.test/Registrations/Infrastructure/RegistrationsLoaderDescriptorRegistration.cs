using LTs.DependencyInjections.Abstractions;
using LTs.DependencyInjections.test.Infrastructure;

namespace LTs.DependencyInjections.test.Registrations.Infrastructure;

internal class RegistrationsLoaderDescriptorRegistration : IRegistration
{
    public void Register( IRegistrationBuilder builder )
        => builder.RegisterDescriptor( new RegistrationDescriptorSample( typeof( IService ), typeof( ServiceImplementation ), "descriptor-tag" ) );
}