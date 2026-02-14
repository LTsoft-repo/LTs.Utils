using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace LTs.Configurations.Exceptions;

/// <summary>
///     Exception thrown when a configuration error occurs.
/// </summary>
[ UsedImplicitly ]
public class ConfigurationException : Exception
{
    /// <summary>
    ///     Creates a new instance of <see cref="ConfigurationException" />.
    /// </summary>
    /// <param name="message">The message of the exception.</param>
    public ConfigurationException( string message )
        : base( message ) { }

    /// <summary>
    ///     Throws an exception if the value is null.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="name">Name of the configuration parameter.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <exception cref="ConfigurationException"></exception>
    public static void ThrowIfNull( [ NotNull ] object? value, string name, IConfiguration configuration )
    {
        if( value is not null )
        {
            return;
        }

        ThrowNullException( name, configuration );
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    /// <summary>
    ///     Throws an exception if the value is null.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="message">Exception message.</param>
    /// <exception cref="ConfigurationException"></exception>
    [ UsedImplicitly ]
    public static void ThrowIfNull( [ NotNull ] object? value, string message )
    {
        if( value is not null )
        {
            return;
        }

        ThrowException( message );
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    /// <summary>
    ///     Throws an exception if the value is null or empty.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="name">Name of the configuration parameter.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <exception cref="ConfigurationException"></exception>
    public static void ThrowIfNullOrEmpty( [ NotNull ] string? value, string name, IConfiguration configuration )
    {
        if( string.IsNullOrEmpty( value ) )
        {
            ThrowNullOrEmptyException( name, configuration );
        }
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    /// <summary>
    ///     Throws an exception if the value is null or empty.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="message">Exception message.</param>
    /// <exception cref="ConfigurationException"></exception>
    public static void ThrowIfNullOrEmpty( [ NotNull ] string? value, string message )
    {
        if( string.IsNullOrEmpty( value ) )
        {
            ThrowException( message );
        }
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    /// <summary>
    ///     Throws an exception if the value is null or whitespace.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="name">Name of the configuration parameter.</param>
    /// <param name="configuration">The configuration object.</param>
    /// <exception cref="ConfigurationException"></exception>
    public static void ThrowIfNullOrWhiteSpace( [ NotNull ] string? value, string name, IConfiguration configuration )
    {
        if( string.IsNullOrWhiteSpace( value ) )
        {
            ThrowNullOrEmptyException( name, configuration );
        }
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    /// <summary>
    ///     Throws an exception if the value is null or whitespace.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <param name="message">Exception message.</param>
    /// <exception cref="ConfigurationException"></exception>
    public static void ThrowIfNullOrWhiteSpace( [ NotNull ] string? value, string message )
    {
        if( string.IsNullOrWhiteSpace( value ) )
        {
            ThrowException( message );
        }
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
    }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

    private static void ThrowNullOrEmptyException( string name, IConfiguration configuration )
    {
        var sectionPath = configuration.GetSectionPath( name );

        ThrowException( $"Configuration parameter '{sectionPath}' cannot be null or empty." );
    }

    private static void ThrowNullException( string name, IConfiguration configuration )
    {
        var sectionPath = configuration.GetSectionPath( name );

        ThrowException( $"Configuration parameter '{sectionPath}' not defined." );
    }

    private static void ThrowException( string message )
        => throw new ConfigurationException( message );
}