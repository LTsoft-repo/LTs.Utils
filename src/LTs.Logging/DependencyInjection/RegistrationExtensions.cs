using Autofac;
using JetBrains.Annotations;
using LTs.DependencyInjections.Extensions;
using LTs.Logging.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LTs.Logging.DependencyInjection;

/// <summary>
///     Extensions for the Autofac container builder.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    ///     Adds Serilog to the Autofac container.
    /// </summary>
    /// <param name="builder">Autofac container builder.</param>
    /// <param name="configuration">Configuration for the logs.</param>
    /// <returns>Autofac container builder.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    [ UsedImplicitly ]
    public static ContainerBuilder AddSerilog( this ContainerBuilder builder, LogConfiguration configuration )
    {
        ArgumentNullException.ThrowIfNull( configuration, nameof( configuration ) );

        var services = new ServiceCollection();
        services.AddLogging( b => b.AddSerilog( dispose: true ) );
        builder.Populate( services );

        LogConfigurator.Configure( configuration );

        return builder;
    }

    /// <summary>
    ///     Adds the <see cref="LogConfiguration" /> loaded from <see cref="IConfiguration" />.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    /// <param name="sectionName">The name of the configuration section.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddLogConfiguration( this ContainerBuilder builder, string sectionName )
    {
        builder.Register( c => c.Resolve<IConfiguration>()
                                .GetSection( sectionName )
                                .LoadLogConfiguration() )
               .SingleInstance();

        return builder;
    }

    /// <summary>
    ///     Registers the <see cref="LogConfiguration" /> loaded from <see cref="IConfiguration" />.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    /// <param name="sectionName">The name of the configuration section.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    [ Obsolete( "Use AddLogConfiguration instead." ) ]
    public static ContainerBuilder RegisterLogConfiguration( this ContainerBuilder builder, string sectionName )
        => builder.AddLogConfiguration( sectionName );
}
