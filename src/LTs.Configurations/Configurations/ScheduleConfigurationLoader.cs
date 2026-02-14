using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Extension methods to load the configuration for <see cref="ScheduleConfiguration" />.
/// </summary>
public static class ScheduleConfigurationLoader
{
    /// <summary>
    ///     Loads the configuration for <see cref="ScheduleConfiguration" />.
    /// </summary>
    /// <param name="configuration">Configuration to load the values from.</param>
    /// <returns>A <see cref="ScheduleConfiguration" /> laoded from the configuration.</returns>
    public static ScheduleConfiguration LoadScheduleConfiguration( this IConfiguration configuration )
        => new()
        {
            TimeInMilliseconds = configuration.GetRequiredValue<int>( nameof( ScheduleConfiguration.TimeInMilliseconds ) )
        };

    /// <summary>
    ///     Loads the configuration for <see cref="ScheduleConfiguration" /> for the specified type.
    /// </summary>
    /// <typeparam name="T">Type the Schedule Configuration is for.</typeparam>
    /// <param name="configuration">Configuration to load the values from.</param>
    /// <returns>A <see cref="ScheduleConfiguration{T}" /> laoded from the configuration.</returns>
    public static ScheduleConfiguration<T> LoadScheduleConfiguration<T>( this IConfiguration configuration )
        where T : class
    {
        var untypedConfiguration = configuration.LoadScheduleConfiguration();

        var typedConfiguration = new ScheduleConfiguration<T>
        {
            TimeInMilliseconds = untypedConfiguration.TimeInMilliseconds
        };

        return typedConfiguration;
    }
}