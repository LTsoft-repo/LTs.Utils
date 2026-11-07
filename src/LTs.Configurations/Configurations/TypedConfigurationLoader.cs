using System.Collections;
using System.Reflection;
using Autofac;
using LTs.Configurations.Attributes;
using LTs.Configurations.Exceptions;
using LTs.Configurations.Extensions;
using LTs.Utils.Extensions.Types;
using LTs.Utils.Reflection;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Loads typed configuration objects from <see cref="IConfiguration" /> using property metadata.
/// </summary>
public static class TypedConfigurationLoader
{
    /// <summary>
    ///     Loads a typed configuration object from a configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="section">The configuration section.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration section )
        where T : notnull
        => section.LoadConfiguration<T>( options: null );

    /// <summary>
    ///     Loads a typed configuration object from the specified configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="configuration">Root application configuration.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration configuration,
                                          string sectionName )
        where T : notnull
        => configuration.LoadConfiguration<T>( sectionName, null );

    /// <summary>
    ///     Loads a typed configuration object from the specified configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="configuration">Root application configuration.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <param name="configureOptions">An optional action that configures per-property load behavior.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration configuration,
                                          string sectionName,
                                          Action<LoadConfigurationOptions<T>>? configureOptions )
        where T : notnull
    {
        var section = configuration.GetSection( sectionName );

        if( !section.Exists() )
        {
            throw new ConfigurationException( $"Configuration section '{sectionName}' not defined." );
        }

        var options = BuildOptions( configureOptions );

        return section.LoadConfiguration( options );
    }

    /// <summary>
    ///     Loads a typed configuration object from a configuration section.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="section">The configuration section.</param>
    /// <param name="options">Optional per-property load behavior.</param>
    /// <returns>The loaded configuration.</returns>
    public static T LoadConfiguration<T>( this IConfiguration section,
                                          LoadConfigurationOptions<T>? options )
        where T : notnull
    {
        var instance = ReflectionHelper.CreateInstance<T>();
        var properties = GetConfigurationProperties( typeof( T ) );

        foreach( var property in properties )
        {
            var defaultValue = property.GetValue( instance );
            var value = ReadConfigurationValue( section, property, defaultValue, options );

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
        where T : notnull
        => builder.AddConfiguration<T>( sectionName, null, null );

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
        where T : notnull
        => builder.AddConfiguration( sectionName, null, configure );

    /// <summary>
    ///     Registers a typed configuration object loaded from configuration.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <param name="builder">Autofac container builder.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <param name="configureOptions">An optional action that configures per-property load behavior.</param>
    /// <param name="configure">An optional post-load configuration action.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddConfiguration<T>( this ContainerBuilder builder,
                                                        string sectionName,
                                                        Action<LoadConfigurationOptions<T>>? configureOptions,
                                                        Func<T, T>? configure )
        where T : notnull
    {
        var options = BuildOptions( configureOptions );

        builder.Register( context =>
                   {
                       var configuration = context.Resolve<IConfiguration>();
                       var section = configuration.GetSection( sectionName );

                       if( !section.Exists() )
                       {
                           throw new ConfigurationException( $"Configuration section '{sectionName}' not defined." );
                       }

                       var loaded = section.LoadConfiguration( options );

                       return configure is null
                                  ? loaded
                                  : configure( loaded );
                   } )
               .As<T>()
               .SingleInstance();

        return builder;
    }

    private static IEnumerable<PropertyInfo> GetConfigurationProperties( Type configurationType )
        => configurationType.GetProperties( BindingFlags.Instance | BindingFlags.Public )
                            .Where( property => property is { GetMethod.IsStatic: false, SetMethod: not null } );

    private static (bool IsRequired, bool AllowEmpty) GetRequiredMetadata( PropertyInfo property )
    {
        var required = property.GetCustomAttribute<RequiredAttribute>();

        return required is null
                   ? ( false, false )
                   : ( true, required.AllowEmpty );
    }

    private static LoadConfigurationOptions<T>? BuildOptions<T>( Action<LoadConfigurationOptions<T>>? configureOptions )
        where T : notnull
    {
        if( configureOptions is null )
        {
            return null;
        }

        var options = new LoadConfigurationOptions<T>();
        configureOptions( options );

        return options;
    }

    private static object? ReadConfigurationValue<T>( IConfiguration section,
                                                      PropertyInfo property,
                                                      object? defaultValue,
                                                      LoadConfigurationOptions<T>? options )
        where T : notnull
    {
        var propertyName = property.Name;
        var propertyType = property.PropertyType;
        var customConverter = TryGetCustomConverter( options, propertyName );
        var isConfigured = HasConfigurationValue( section, propertyName );
        var (isRequired, allowEmpty) = GetRequiredMetadata( property );

        if( !isConfigured )
        {
            if( isRequired && !allowEmpty )
            {
                ConfigurationException.ThrowIfNull( null, propertyName, section );
            }

            if( customConverter is null )
            {
                return defaultValue;
            }

            var missingValue = ConvertConfiguration( section,
                                                     property,
                                                     customConverter.Value.configValueType,
                                                     customConverter.Value.converter,
                                                     false,
                                                     defaultValue );

            if( isRequired )
            {
                ValidateRequiredValue( section, propertyName, propertyType, missingValue, allowEmpty );
            }

            return missingValue ?? defaultValue;
        }

        var value = customConverter is not null
                        ? ConvertConfiguration( section,
                                                property,
                                                customConverter.Value.configValueType,
                                                customConverter.Value.converter,
                                                true,
                                                defaultValue )
                        : ConvertPropertyValue( section, propertyName, propertyType, options );

        if( isRequired )
        {
            ValidateRequiredValue( section, propertyName, propertyType, value, allowEmpty );
        }

        return value ?? defaultValue;
    }

    private static (Type configValueType, Func<object?, object?> converter)? TryGetCustomConverter<T>(
        LoadConfigurationOptions<T>? options,
        string propertyName )
        where T : notnull
    {
        if( options is null || !options.TryGetConverter( propertyName, out var configValueType, out var converter ) )
        {
            return null;
        }

        return ( configValueType!, converter! );
    }

    private static object? ConvertConfiguration( IConfiguration section,
                                                 PropertyInfo property,
                                                 Type configValueType,
                                                 Func<object?, object?> convert,
                                                 bool isConfigured,
                                                 object? defaultValue )
    {
        var propertyName = property.Name;
        var propertyType = property.PropertyType;

        try
        {
            var configValue = ReadValue( section,
                                         propertyName,
                                         configValueType,
                                         isConfigured,
                                         defaultValue );

            return convert( configValue );
        }
        catch( ConfigurationException )
        {
            throw;
        }
        catch( Exception ex )
        {
            throw new ConfigurationException(
                $"Configuration parameter '{section.GetSectionPath( propertyName )}' is not of type '{propertyType.Name}'.\n" +
                $"Exception message: {ex.Message}" );
        }
    }

    private static object? ReadValue( IConfiguration section,
                                      string propertyName,
                                      Type configValueType,
                                      bool isConfigured,
                                      object? defaultValue )
    {
        var underlyingType = Nullable.GetUnderlyingType( configValueType ) ?? configValueType;

        if( !isConfigured )
        {
            return underlyingType == typeof( string ) ? null : defaultValue;
        }

        if( underlyingType == typeof( string ) )
        {
            return section[ propertyName ];
        }

        return ConvertPropertyValue( section, propertyName, configValueType, null );
    }

    private static bool HasConfigurationValue( IConfiguration section, string propertyName )
    {
        var propertySection = section.GetSection( propertyName );

        if( propertySection.Exists() )
        {
            return true;
        }

        if( section.GetChildren().Any( child =>
                                           string.Equals( child.Key, propertyName, StringComparison.OrdinalIgnoreCase ) ) )
        {
            return true;
        }

        var propertyPath = section.GetSectionPath( propertyName );

        return section.AsEnumerable().Any( pair =>
                                               string.Equals( pair.Key, propertyPath, StringComparison.OrdinalIgnoreCase ) );
    }

    private static void ValidateRequiredValue( IConfiguration section,
                                               string propertyName,
                                               Type propertyType,
                                               object? value,
                                               bool allowEmpty )
    {
        var underlyingType = Nullable.GetUnderlyingType( propertyType ) ?? propertyType;

        if( value is null )
        {
            ConfigurationException.ThrowIfNull( null, propertyName, section );
        }

        if( underlyingType == typeof( string ) )
        {
            if( allowEmpty )
            {
                return;
            }

            ConfigurationException.ThrowIfNullOrWhiteSpace( (string?)value, propertyName, section );

            return;
        }

        if( underlyingType.IsCollectionType() && !allowEmpty && IsEmptyCollection( value! ) )
        {
            ConfigurationException.ThrowIfNullOrEmpty( string.Empty, propertyName, section );
        }
    }

    private static bool IsEmptyCollection( object value )
        => value switch
        {
            Array array => array.Length == 0,
            ICollection collection => collection.Count == 0,
            _ => false
        };

    private static readonly MethodInfo LoadConfigurationWithOptionsDefinition = typeof( TypedConfigurationLoader )
        .GetMethods( BindingFlags.Public | BindingFlags.Static )
        .Single( method => method is { Name: nameof( LoadConfiguration ), IsGenericMethodDefinition: true }
                             && method.GetParameters() is [ _, { ParameterType.IsGenericType: true } parameter ]
                             && parameter.ParameterType.GetGenericTypeDefinition() == typeof( LoadConfigurationOptions<> ) );

    private static object? ConvertPropertyValue( IConfiguration section,
                                               string propertyName,
                                               Type propertyType,
                                               object? loadOptions )
    {
        var underlyingType = Nullable.GetUnderlyingType( propertyType ) ?? propertyType;
        var propertySection = section.GetSection( propertyName );

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

            return underlyingType.IsCollectionType()
                       ? ConvertCollectionValue( propertyType, propertySection )
                       : LoadNestedConfiguration( propertySection, underlyingType, loadOptions, propertyName );
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

    private static object ConvertCollectionValue( Type propertyType,
                                                  IConfigurationSection propertySection )
    {
        if( propertySection.Value is not null
            && !propertySection.GetChildren().Any()
            && !string.IsNullOrEmpty( propertySection.Value ) )
        {
            throw new ConfigurationException( $"Can not convert to type '{propertyType.Name}'" );
        }

        return propertySection.Get( propertyType ) ?? CreateEmptyCollection( propertyType );
    }

    private static object CreateEmptyCollection( Type propertyType )
    {
        if( propertyType.IsArray )
        {
            return Array.CreateInstance( propertyType.GetElementType()!, 0 );
        }

        if( propertyType.IsGenericType )
        {
            var elementType = propertyType.GetGenericArguments()[ 0 ];
            var listType = typeof( List<> ).MakeGenericType( elementType );

            return Activator.CreateInstance( listType )!;
        }

        throw new ConfigurationException( $"Configuration type '{propertyType.Name}' is not supported." );
    }

    private static object LoadNestedConfiguration( IConfiguration section,
                                                 Type configurationType,
                                                 object? parentLoadOptions,
                                                 string nestedPropertyName )
    {
        object? nestedLoadOptions = null;

        if( parentLoadOptions is not null )
        {
            var createMethod = parentLoadOptions.GetType().GetMethod(
                "CreateNestedLoadOptions",
                BindingFlags.Instance | BindingFlags.NonPublic );

            nestedLoadOptions = createMethod?.Invoke( parentLoadOptions, [ configurationType, nestedPropertyName ] );
        }

        var genericLoadMethod = LoadConfigurationWithOptionsDefinition.MakeGenericMethod( configurationType );

        try
        {
            var result = genericLoadMethod.Invoke( null, [ section, nestedLoadOptions ] );

            return result
                   ?? throw new ConfigurationException( $"Configuration type '{configurationType.Name}' is not supported." );
        }
        catch( TargetInvocationException ex ) when( ex.InnerException is ConfigurationException configurationException )
        {
            throw configurationException;
        }
    }
}