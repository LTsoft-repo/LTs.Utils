using Autofac;
using LTs.DependencyInjections.Abstractions;

#pragma warning disable IDE0290

namespace LTs.DependencyInjections.Registrations;

/// <summary>
///     Autofac-backed <see cref="IRegistrationBuilder" />.
/// </summary>
public class RegistrationBuilder : IRegistrationBuilder
{
    private readonly ContainerBuilder builder;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RegistrationBuilder" /> class.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    public RegistrationBuilder( ContainerBuilder builder )
        => this.builder = builder;

    /// <inheritdoc />
    public void RegisterSingleton<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull, TService
        => builder.RegisterType<TImplementation>()
                  .As<TService>()
                  .SingleInstance();

    /// <inheritdoc />
    public void RegisterDescriptor( IRegistrationDescriptor descriptor )
        => builder.RegisterInstance( descriptor )
                  .As<IRegistrationDescriptor>()
                  .SingleInstance();
}