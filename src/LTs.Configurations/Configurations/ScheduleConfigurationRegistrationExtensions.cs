using Autofac;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Extension methods for registering the Schedule Configuration.
/// </summary>
public static class ScheduleConfigurationRegistrationExtensions
{
    /// <summary>
    ///     Registers the <see cref="ScheduleConfiguration" /> loaded from <see cref="IConfiguration" />.
    /// </summary>
    /// <param name="builder">ContainerBuilder to register the services.</param>
    /// <param name="sectionName">Name of the section in the configuration.</param>
    /// <returns>The container builder with the added configuration.</returns>
    public static ContainerBuilder AddScheduleConfiguration( this ContainerBuilder builder, string sectionName )
    {
        builder.Register( c =>
                              c.Resolve<IConfiguration>()
                               .GetSection( sectionName )
                               .LoadScheduleConfiguration() )
               .As<ScheduleConfiguration>()
               .SingleInstance();

        return builder;
    }

    /// <summary>
    ///     Registers the <see cref="ScheduleConfiguration{T}" /> loaded from <see cref="IConfiguration" />.
    /// </summary>
    /// r
    /// <typeparam name="T">Type the Schedule Configuration is for.</typeparam>
    /// <param name="builder">ContainerBuilder to register the services.</param>
    /// <param name="sectionName">Name of the section in the configuration.</param>
    /// <returns>The container builder with the added configuration.</returns>
    public static ContainerBuilder AddScheduleConfiguration<T>( this ContainerBuilder builder, string sectionName )
        where T : class
    {
        builder.Register( c =>
                              c.Resolve<IConfiguration>()
                               .GetSection( sectionName )
                               .LoadScheduleConfiguration<T>() )
               .As<ScheduleConfiguration<T>>()
               .SingleInstance();

        return builder;
    }
}