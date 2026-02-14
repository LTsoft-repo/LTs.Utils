using System.Text;
using LTs.Configurations.Exceptions;
using LTs.Utils.Extensions.Types;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Extensions;

/// <summary>
///     Extensions to get values from <see cref="IConfiguration" />.
/// </summary>
public static class GetConfigurationExtensions
{
    /// <summary>
    ///     Gets the value of the configuration parameter with the specified name.
    /// </summary>
    /// <typeparam name="T">The type of the configuration parameter.</typeparam>
    /// <param name="configuration">The configuration object.</param>
    /// <param name="name">The configuration name to get the value from.</param>
    /// <returns>The value of the configuration parameter.</returns>
    /// <exception cref="ConfigurationException"></exception>
    public static T GetRequiredValue<T>( this IConfiguration configuration, string name )
    {
        // Checks if the section actually exists.
        var section = configuration.GetSection( name );

        if( !section.Exists() )
        {
            ThrowMissingConfigurationIfNull( configuration, name, (T?)default );
        }

        var result = (T?)default;

        // Basic types.
        if( typeof( T ).IsBasicType() )
        {
            var value = configuration.GetValue( name, (string?)null );
            ThrowMissingConfigurationIfNull( configuration, name, value );

            result = configuration.GetValue( name, (T?)default )!;
        }

        // Other types.
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if( result is null )
        {
            try
            {
                result = section.Get<T>();

                if( result.IsDefault() )
                {
                    throw new ConfigurationException( $"Can not convert to type '{typeof( T ).Name}'" );
                }
            }
            catch( Exception ex )
            {
                throw new ConfigurationException( $"Configuration parameter '{name}' is not of type '{typeof( T ).Name}'.\n" +
                                                  $"Exception message: {ex.Message}\n" +
                                                  $"Section content: \n" +
                                                  $"{GetSectionContent( section )}" );
            }
        }

        ThrowMissingConfigurationIfNull( configuration, name, result );

        return result!; // Does not return null because of the exception thrown if it is null.
    }

    /// <summary>
    ///     Gets the connection string with the specified name.
    /// </summary>
    /// <param name="configuration">The configuration object.</param>
    /// <param name="name">The configuration name to get the value from.</param>
    /// <returns>The connection string.</returns>
    /// <exception cref="ConfigurationException"></exception>
    public static string GetRequiredConnectionString( this IConfiguration configuration, string name )
    {
        var connectionString = configuration.GetConnectionString( name );

        ThrowMissingConnectionIfNullOrEmpty( name, connectionString );

        return connectionString!; // Does not return null because of the exception thrown if it is null or empty.
    }

    /// <summary>
    ///     Gets the full path of the section.
    /// </summary>
    /// <param name="configuration">Configuration object that contains the section.</param>
    /// <param name="name">Name of the section.</param>
    /// <returns>The full path of the section.</returns>
    public static string GetSectionPath( this IConfiguration configuration, string name )
        => configuration is IConfigurationSection section
               ? $"{section.Path}:{name}"
               : name;

    /// <summary>
    ///     Throws a <see cref="ConfigurationException" /> if the value is null.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="configuration">Configuration where the value is read from.</param>
    /// <param name="name">Name of the configuration parameter.</param>
    /// <param name="value">Value of the configuration parameter.</param>
    /// <exception cref="ConfigurationException"></exception>
    private static void ThrowMissingConfigurationIfNull<T>( IConfiguration configuration, string name, [ NotNull ] T? value )
        => ConfigurationException.ThrowIfNull( value, name, configuration );

    /// <summary>
    ///     Throws a <see cref="ConfigurationException" /> if the connection string is null.
    /// </summary>
    /// <param name="name">Name of the configuration parameter.</param>
    /// <param name="value">Value of the configuration parameter.</param>
    /// <exception cref="ConfigurationException"></exception>
    private static void ThrowMissingConnectionIfNullOrEmpty( string name, string? value )
        => ConfigurationException.ThrowIfNullOrWhiteSpace( value, $"Connection String '{name}' not defined." );

    /// <summary>
    ///     Gets the full content of the section.
    /// </summary>
    /// <param name="section">Section to get the content from.</param>
    /// <returns>The full content of the section.</returns>
    private static string GetSectionContent( IConfiguration section )
    {
        var sb = new StringBuilder();

        foreach( var pair in section.AsEnumerable() )
        {
            if( pair.Value is not null )
            {
                sb.AppendLine( $"\t{pair.Key} = {pair.Value}" );
            }
        }

        return sb.ToString().TrimEnd( '\n' ).TrimEnd( '\r' );
    }
}