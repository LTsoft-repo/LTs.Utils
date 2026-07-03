using Autofac;
using LTs.Configurations.Abstractions;
using LTs.DependencyInjections.Extensions;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = LTs.Configurations.Abstractions.IConfigurationProvider;

namespace LTs.Configurations.DependencyInjection;

/// <summary>
///     Extensions to register the configuration provider with Autofac.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    ///     Registers the Autofac configuration provider and the IConfiguration.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="configurationProvider">Configuration provider instance to register.</param>
    /// <returns>Container builder with the configuration provider registered.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddConfigurationProvider( this ContainerBuilder builder,
                                                             IAutofacConfigurationProvider configurationProvider )
        => builder.AddConfigurationProvider( configurationProvider, null );

    /// <summary>
    ///     Registers the configuration provider and the IConfiguration.
    /// </summary>
    /// <param name="builder">Container builder.</param>
    /// <param name="configurationProvider">Configuration provider instance to register.</param>
    /// <param name="registerConfigurationsAction">
    ///     Action to register the configurations as services (required if the provider is not
    ///     IAutofacConfigurationProvider).
    /// </param>
    /// <returns>Container builder with the configuration provider registered.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [ UsedImplicitly ]
    public static ContainerBuilder AddConfigurationProvider( this ContainerBuilder builder,
                                                             IConfigurationProvider configurationProvider,
                                                             Action<ContainerBuilder>? registerConfigurationsAction )
    {
        builder.RegisterInstance( configurationProvider )
               .As<IConfigurationProvider>()
               .SingleInstance();

        // If it is an AutofacConfigurationProvider, it will use the container to register the configurations.
        if( configurationProvider is IAutofacConfigurationProvider autofacConfigurationProvider )
        {
            builder.RegisterInstance( configurationProvider )
                   .As<IAutofacConfigurationProvider>()
                   .SingleInstance();

            var container = autofacConfigurationProvider.GetContainer();
            builder.Populate( container );
        }
        else
        {
            // If it is not an AutofacConfigurationProvider, it will use the action to register the configurations.
            ArgumentNullException.ThrowIfNull( registerConfigurationsAction );
            registerConfigurationsAction.Invoke( builder );
        }

        builder.Register( _ => configurationProvider.Get<IConfigurationRoot>() )
               .As<IConfigurationRoot>()
               .As<IConfiguration>()
               .SingleInstance();

        return builder;
    }
}