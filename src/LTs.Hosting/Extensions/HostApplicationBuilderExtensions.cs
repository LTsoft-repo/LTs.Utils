using Autofac;
using JetBrains.Annotations;
using LTs.Configurations;
using LTs.Configurations.Abstractions;
using LTs.Configurations.DependencyInjection;
using LTs.Configurations.Extensions;
using LTs.DependencyInjections.DependencyInjection;
using LTs.Logging;
using LTs.Logging.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LTs.Hosting.Extensions;

/// <summary>
///     Extension to configure a default host application for Windows Service.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Configures a default host application for Windows Service.
    ///     <para>
    ///         <list type="bullet">
    ///             <item>Adds Autofac for DI</item>
    ///             <item>Adds Serilog for logging</item>
    ///             <item>Registers a ServiceProvider</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Needs to have a <see cref="LogConfiguration" /> registered.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     Loads the configuration in the following order:
    ///     <list type="bullet">
    ///         <item>appsettings.json</item>
    ///         <item>appsettings.&lt;Environment&gt;.json</item>
    ///         <item>User Secrets</item>
    ///         <item>Key per File</item>
    ///         <item>Environment Variables</item>
    ///         <item>Additional configuration (if any)</item>
    ///     </list>
    ///     <para>
    ///         <b>IMPORTANT: If <paramref name="serviceName" /> is empty, will not register Windows Service.</b>
    ///     </para>
    /// </remarks>
    /// <param name="builder"><see cref="IHostApplicationBuilder" /> to create the host.</param>
    /// <param name="serviceName">Name of the Windows Service.</param>
    /// <param name="registerConfigurationAction">Action to register the configuration types.</param>
    /// <param name="configureServicesAction">Action to register the services with Autofac.</param>
    /// <param name="serviceCollectionAction">
    ///     Action to register the services with Microsoft DI. Mainly to use
    ///     <c>.AddHostedService&lt;T&gt;()</c> to register the Worker.
    /// </param>
    /// <returns>A <see cref="IHostApplicationBuilder" />.</returns>
    [ UsedImplicitly ]
    public static IHostApplicationBuilder ConfigureDefaultHostApplication<T>( this IHostApplicationBuilder builder,
                                                                              string serviceName,
                                                                              Action<ContainerBuilder> registerConfigurationAction,
                                                                              Action<ContainerBuilder, IAutofacConfigurationProvider>
                                                                                  configureServicesAction,
                                                                              Action<IServiceCollection> serviceCollectionAction )
        where T : class
        => builder.ConfigureDefaultHostApplication<T>(
            serviceName,
            registerConfigurationAction,
            configureServicesAction,
            serviceCollectionAction,
            null );

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Configures a default host application for Windows Service.
    ///     <para>
    ///         <list type="bullet">
    ///             <item>Adds Autofac for DI</item>
    ///             <item>Adds Serilog for logging</item>
    ///             <item>Registers a ServiceProvider</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Needs to have a <see cref="LogConfiguration" /> registered.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     Loads the configuration in the following order:
    ///     <list type="bullet">
    ///         <item>appsettings.json</item>
    ///         <item>appsettings.&lt;Environment&gt;.json</item>
    ///         <item>User Secrets</item>
    ///         <item>Key per File</item>
    ///         <item>Environment Variables</item>
    ///         <item>Additional configuration (if any)</item>
    ///     </list>
    ///     <para>
    ///         <b>IMPORTANT: If <paramref name="serviceName" /> is empty, will not register Windows Service.</b>
    ///     </para>
    /// </remarks>
    /// <param name="builder"><see cref="IHostApplicationBuilder" /> to create the host.</param>
    /// <param name="serviceName">Name of the Windows Service.</param>
    /// <param name="registerConfigurationAction">Action to register the configuration types.</param>
    /// <param name="configureServicesAction">Action to register the services with Autofac.</param>
    /// <param name="serviceCollectionAction">
    ///     Action to register the services with Microsoft DI. Mainly to use
    ///     <c>.AddHostedService&lt;T&gt;()</c> to register the Worker.
    /// </param>
    /// <param name="actionAdditionalLoggingConfiguration">Additional configuration for Serilog.</param>
    /// <returns>A <see cref="IHostApplicationBuilder" />.</returns>
    [ UsedImplicitly ]
    public static IHostApplicationBuilder ConfigureDefaultHostApplication<T>( this IHostApplicationBuilder builder,
                                                                              string serviceName,
                                                                              Action<ContainerBuilder> registerConfigurationAction,
                                                                              Action<ContainerBuilder, IAutofacConfigurationProvider>
                                                                                  configureServicesAction,
                                                                              Action<IServiceCollection> serviceCollectionAction,
                                                                              Action<LoggerConfiguration, IAutofacConfigurationProvider>?
                                                                                  actionAdditionalLoggingConfiguration )
        where T : class
        => builder.ConfigureDefaultHostApplication(
            serviceName,
            configBuilder => configBuilder.AddDefaultConfigurationForAssembly<T>( _ => { } ),
            registerConfigurationAction,
            configureServicesAction,
            serviceCollectionAction,
            actionAdditionalLoggingConfiguration );

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Configures a default host application for Windows Service.
    ///     <para>
    ///         <list type="bullet">
    ///             <item>Adds Autofac for DI</item>
    ///             <item>Adds Serilog for logging</item>
    ///             <item>Registers a ServiceProvider</item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         Needs to have a <see cref="LogConfiguration" /> registered.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     <b>IMPORTANT: If <paramref name="serviceName" /> is empty, will not register Windows Service.</b>
    /// </remarks>
    /// <param name="builder"><see cref="IHostApplicationBuilder" /> to create the host.</param>
    /// <param name="serviceName">Name of the Windows Service.</param>
    /// <param name="loadConfigurationAction">Action to load the configuration.</param>
    /// <param name="registerConfigurationAction">Action to register the configuration types.</param>
    /// <param name="configureServicesAction">Action to register the services with Autofac.</param>
    /// <param name="serviceCollectionAction">
    ///     Action to register the services with Microsoft DI. Mainly to use
    ///     <c>.AddHostedService&lt;T&gt;()</c> to register the Worker.
    /// </param>
    /// <param name="actionAdditionalLoggingConfiguration">Additional configuration for Serilog.</param>
    /// <returns>A <see cref="IHostApplicationBuilder" />.</returns>
    [ UsedImplicitly ]
    public static IHostApplicationBuilder ConfigureDefaultHostApplication( this IHostApplicationBuilder builder,
                                                                           string serviceName,
                                                                           Action<IConfigurationBuilder> loadConfigurationAction,
                                                                           Action<ContainerBuilder> registerConfigurationAction,
                                                                           Action<ContainerBuilder, IAutofacConfigurationProvider>
                                                                               configureServicesAction,
                                                                           Action<IServiceCollection> serviceCollectionAction,
                                                                           Action<LoggerConfiguration, IAutofacConfigurationProvider>?
                                                                               actionAdditionalLoggingConfiguration )
    {
        var configurationBuilder = new ConfigurationBuilder();

        loadConfigurationAction( configurationBuilder );
        var configuration = configurationBuilder.Build();

        // Configuration Provider.
        var configurationProvider = new AutofacConfigurationProvider( configuration, registerConfigurationAction );

        // Logging.
        var logConfiguration = configurationProvider.Get<LogConfiguration>();

        LogConfigurator.Configure(
            logConfiguration,
            loggerConfiguration => actionAdditionalLoggingConfiguration?.Invoke( loggerConfiguration, configurationProvider ) );

        builder.Logging
               .ClearProviders()
               .AddSerilog();

        // Microsoft DI.
        if( !string.IsNullOrWhiteSpace( serviceName ) )
        {
            builder.Services.AddWindowsService( options => options.ServiceName = serviceName );
        }

        // Workers.
        serviceCollectionAction( builder.Services );

        // Autofac.
        builder.ConfigureContainer( new AutofacServiceProviderFactory(),
                                    b =>
                                        {
                                            b.AddConfigurationProvider( configurationProvider );
                                            configureServicesAction( b, configurationProvider );
                                        } );

        return builder;
    }
}
