using Autofac;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Extensions to load <see cref="AppConfigConfiguration" />.
/// </summary>
public static class AppConfigConfigurationLoader
{
    /// <summary>
    ///     Loads the configuration for <see cref="AppConfigConfiguration" /> from the provided <see cref="IConfiguration" />.
    /// </summary>
    /// <param name="configuration">The configuration to load the data from.</param>
    /// <returns>The loaded <see cref="AppConfigConfiguration" />.</returns>
    public static AppConfigConfiguration LoadAppConfigConfiguration( this IConfiguration configuration )
        => new()
        {
            ConnectionString = configuration.GetConnectionString( "AppConfig" )
        };

    /// <summary>
    ///     Registers the <see cref="AppConfigConfiguration" /> loaded from <see cref="IConfiguration" />.
    ///     <para>The configuration is loaded the first time the object is requested.</para>
    /// </summary>
    /// <param name="builder">The container builder.</param>
    /// <returns>The container builder with the added configuration.</returns>
    public static ContainerBuilder AddAppConfigConfigurationLoader( this ContainerBuilder builder )
    {
        builder.Register( context => context.Resolve<IConfiguration>()
                                             .LoadAppConfigConfiguration() )
               .SingleInstance();

        return builder;
    }
}
