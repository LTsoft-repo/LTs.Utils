using Autofac;
using LTs.Configurations.Abstractions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations;

/// <summary>
///     Configuration provider default implementation with Autofac.
/// </summary>
[ UsedImplicitly ]
//public class AutofacConfigurationProvider : ConfigurationProvider, IAutofacConfigurationProvider
public class AutofacConfigurationProvider : ConfigurationProvider, IAutofacConfigurationProvider
{
    /// <summary>
    ///     Creates a new instance of the <see cref="AutofacConfigurationProvider" /> class, registering the IConfiguration
    ///     instance, and any additional configuration.
    /// </summary>
    /// <param name="configuration">The configuration instance to register.</param>
    /// <param name="registerConfiguration">Action to register additional configuration.</param>
    public AutofacConfigurationProvider( IConfigurationRoot configuration,
                                         Action<ContainerBuilder> registerConfiguration )
        : base( configuration, registerConfiguration ) { }

    /// <inheritdoc />
    public virtual IContainer GetContainer() => DiContainer;
}