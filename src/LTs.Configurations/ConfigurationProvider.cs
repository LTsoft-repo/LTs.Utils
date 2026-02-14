using Autofac;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = LTs.Configurations.Abstractions.IConfigurationProvider;

namespace LTs.Configurations;

/// <summary>
///     Default Implementation of the <see cref="IConfigurationProvider" /> interface.
/// </summary>
[ UsedImplicitly ]
public class ConfigurationProvider : IConfigurationProvider
{
    /// <summary>
    ///     Autofac container for configuration services.
    /// </summary>
    [ UsedImplicitly ]
    protected readonly IContainer DiContainer;

    /// <summary>
    ///     Creates a new instance of the <see cref="ConfigurationProvider" /> class, registering the IConfiguration
    ///     instance, and any additional configuration.
    /// </summary>
    /// <param name="configuration">The configuration instance to register.</param>
    /// <param name="registerConfiguration">Action to register additional configuration.</param>
    public ConfigurationProvider( IConfigurationRoot configuration, Action<ContainerBuilder> registerConfiguration )
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register( _ => configuration ).As<IConfigurationRoot>().SingleInstance();
        containerBuilder.Register( _ => configuration ).As<IConfiguration>().SingleInstance();

        registerConfiguration( containerBuilder );

        DiContainer = containerBuilder.Build();
    }

    /// <inheritdoc />
    public virtual T Get<T>() where T : notnull => DiContainer.Resolve<T>();
}