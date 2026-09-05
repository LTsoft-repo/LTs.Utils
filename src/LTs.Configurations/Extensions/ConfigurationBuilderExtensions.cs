using System.Text;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Extensions;

/// <summary>
///     Extensions for <see cref="IConfigurationBuilder" />.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    ///     Adds a JSON configuration source from a JSON string.
    /// </summary>
    /// <param name="builder">Configuration builder instance.</param>
    /// <param name="jsonString">JSON content as a string.</param>
    /// <returns>The <see cref="IConfigurationBuilder" /> instance with the added JSON configuration source.</returns>
    public static IConfigurationBuilder AddJsonString( this IConfigurationBuilder builder, string jsonString )
        => builder.AddJsonStream( new MemoryStream( Encoding.UTF8.GetBytes( jsonString ) ) );
}
