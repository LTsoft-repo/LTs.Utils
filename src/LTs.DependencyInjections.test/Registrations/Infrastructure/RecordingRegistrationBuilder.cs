using LTs.DependencyInjections.Abstractions;

namespace LTs.DependencyInjections.test.Registrations.Infrastructure;

internal class RecordingRegistrationBuilder : IRegistrationBuilder
{
    public List<(Type Service, Type Implementation)> Singletons { get; } = [ ];

    public List<IRegistrationDescriptor> Descriptors { get; } = [ ];

    public void RegisterSingleton<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull, TService
        => Singletons.Add( ( typeof( TService ), typeof( TImplementation ) ) );

    public void RegisterDescriptor( IRegistrationDescriptor descriptor )
        => Descriptors.Add( descriptor );
}