using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Autofac;
using LTs.Configurations.Exceptions;
using LTs.Configurations.Extensions;
using LTs.Utils.Extensions.Types;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Loads typed configuration objects from <see cref="IConfiguration" /> using property metadata.
/// </summary>
public static class TypedConfigurationLoader
{
    /// <summary>
    ///     Loads a typed configuration object from the specified configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="configuration">Root application configuration.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration configuration,
                                          string sectionName )
        where T : notnull, new()
    {
        var section = configuration.GetSection( sectionName );

        if( !section.Exists() )
        {
            throw new ConfigurationException( $"Configuration section '{sectionName}' not defined." );
        }

        return section.LoadConfiguration<T>();
    }

    /// <summary>
    ///     Loads a typed configuration object from a configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="section">The configuration section.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration section )
        where T : notnull, new()
    {
        var instance = new T();
        var properties = GetConfigurationProperties( typeof( T ) );

        foreach( var property in properties )
        {
            var isRequired = Attribute.IsDefined( property, typeof( RequiredAttribute ) );
            var defaultValue = property.GetValue( instance );
            var value = ReadPropertyValue( section, property, isRequired, defaultValue );

            property.SetValue( instance, value );
        }

        return instance;
    }

    /// <summary>
    ///     Registers a typed configuration object loaded from configuration.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="builder">Autofac container builder.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddConfiguration<T>( this ContainerBuilder builder,
                                                        string sectionName )
        where T : notnull, new()
        => builder.AddConfiguration<T>( sectionName, null );

    /// <summary>
    ///     Registers a typed configuration object loaded from configuration.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="builder">Autofac container builder.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <param name="configure">An optional post-load configuration action.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddConfiguration<T>( this ContainerBuilder builder,
                                                        string sectionName,
                                                        Func<T, T>? configure )
        where T : notnull, new()
    {
        builder.Register( context =>
                   {
                       var configuration = context.Resolve<IConfiguration>()
                                                  .LoadConfiguration<T>( sectionName );

                       return configure is null
                                  ? configuration
                                  : configure( configuration );
                   } )
               .As<T>()
               .SingleInstance();

        return builder;
    }

    private static IEnumerable<PropertyInfo> GetConfigurationProperties( Type configurationType )
        => configurationType.GetProperties( BindingFlags.Instance | BindingFlags.Public )
                            .Where( property => property.GetMethod is { IsStatic: false } &&
                                                property.SetMethod is not null );

    private static object? ReadPropertyValue( IConfiguration section,
                                              PropertyInfo property,
                                              bool isRequired,
                                              object? defaultValue )
    {
        var propertyName = property.Name;
        var propertyType = property.PropertyType;

        if( !HasConfiguredValue( section, propertyName ) )
        {
            if( isRequired )
            {
                ConfigurationException.ThrowIfNull( null, propertyName, section );
            }

            return defaultValue;
        }

        var value = ConvertPropertyValue( section, propertyName, propertyType );

        if( isRequired )
        {
            ValidateRequiredValue( section, propertyName, propertyType, value );
        }

        return value ?? defaultValue;
    }

    private static bool HasConfiguredValue( IConfiguration section, string propertyName )
    {
        var propertySection = section.GetSection( propertyName );

        if( propertySection.GetChildren().Any() )
        {
            return true;
        }

        return section[ propertyName ] is not null;
    }

    private static void ValidateRequiredValue( IConfiguration section,
                                               string propertyName,
                                               Type propertyType,
                                               object? value )
    {
        var underlyingType = Nullable.GetUnderlyingType( propertyType ) ?? propertyType;

        if( value is null )
        {
            ConfigurationException.ThrowIfNull( null, propertyName, section );
        }

        if( underlyingType == typeof( string ) && string.IsNullOrWhiteSpace( (string?)value ) )
        {
            ConfigurationException.ThrowIfNullOrWhiteSpace( (string?)value, propertyName, section );
        }
    }

    private static object? ConvertPropertyValue( IConfiguration section,
                                                 string propertyName,
                                                 Type propertyType )
    {
        var underlyingType = Nullable.GetUnderlyingType( propertyType ) ?? propertyType;

        try
        {
            if( underlyingType == typeof( string ) )
            {
                return section[ propertyName ];
            }

            if( underlyingType == typeof( Uri ) )
            {
                var stringValue = section[ propertyName ];

                return stringValue is null
                           ? null
                           : new Uri( stringValue );
            }

            if( underlyingType.IsEnum )
            {
                var stringValue = section[ propertyName ];

                return stringValue is null
                           ? null
                           : Enum.Parse( underlyingType, stringValue, true );
            }

            if( underlyingType.IsBasicType() || underlyingType == typeof( TimeSpan ) || underlyingType == typeof( Guid ) )
            {
                return section.GetValue( underlyingType, propertyName );
            }

            var nestedSection = section.GetSection( propertyName );

            if( nestedSection.GetChildren().Any() )
            {
                throw new ConfigurationException(
                    $"Configuration property '{section.GetSectionPath( propertyName )}' of type '{propertyType.Name}' is not supported." );
            }

            return section.GetValue( underlyingType, propertyName );
        }
        catch( ConfigurationException )
        {
            throw;
        }
        catch( Exception ex )
        {
            throw new ConfigurationException(
                $"Configuration parameter '{section.GetSectionPath( propertyName )}' is not of type '{underlyingType.Name}'.\n" +
                $"Exception message: {ex.Message}" );
        }
    }
}