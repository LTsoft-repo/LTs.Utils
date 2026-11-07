using System.Linq.Expressions;
using System.Reflection;
using LTs.Configurations.Extensions;
using LTs.Utils.Reflection;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Options for customizing how individual properties are loaded by
///     <see cref="TypedConfigurationLoader" />.
/// </summary>
/// <typeparam name="T">The configuration type.</typeparam>
public class LoadConfigurationOptions<T>
    where T : notnull
{
    private readonly Dictionary<string, ( Type valueType, Func<object?, object?> converter )> converters = new( StringComparer.OrdinalIgnoreCase );

    /// <summary>
    ///     Registers a custom converter for a configuration property.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="property">An expression that identifies the property.</param>
    /// <param name="convert">Converts the configuration value for the property.</param>
    /// <returns>The options instance.</returns>
    public LoadConfigurationOptions<T> SetValue<TProperty>( Expression<Func<T, TProperty>> property,
                                                            Func<TProperty?, TProperty> convert )
        => SetValue<TProperty?, TProperty>( property, convert );

    /// <summary>
    ///     Registers a custom converter for a configuration property.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <typeparam name="TConfigValue">The configuration value type.</typeparam>
    /// <param name="property">An expression that identifies the property.</param>
    /// <param name="convert">Converts the configuration value for the property.</param>
    /// <returns>The options instance.</returns>
    public LoadConfigurationOptions<T> SetValue<TConfigValue, TProperty>( Expression<Func<T, TProperty>> property,
                                                                          Func<TConfigValue, TProperty> convert )
    {
        ArgumentNullException.ThrowIfNull( property );
        ArgumentNullException.ThrowIfNull( convert );

        var configurationPath = property.GetConfigurationPath();

        if( converters.ContainsKey( configurationPath ) )
        {
            throw new ArgumentException(
                $"SetValue was already configured for property '{configurationPath}'.",
                nameof( property ) );
        }

        converters[ configurationPath ] = ( typeof( TConfigValue ), value => convert( (TConfigValue)value! ) );

        return this;
    }

    internal bool TryGetConverter( string propertyName,
                                   out Type? configValueType,
                                   out Func<object?, object?>? converter )
    {
        configValueType = null;
        converter = null;

        if( !converters.TryGetValue( propertyName, out var entry ) )
        {
            return false;
        }

        configValueType = entry.valueType;
        converter = entry.converter;

        return true;
    }

    internal void RegisterConverter( string propertyName,
                                     Type configValueType,
                                     Func<object?, object?> convert )
    {
        converters[ propertyName ] = ( configValueType, convert );
    }

    internal object? CreateNestedLoadOptions( Type nestedType, string nestedPropertyName )
    {
        var prefix = nestedPropertyName + ":";
        var matching = converters
            .Where( entry => entry.Key.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
            .ToList();

        if( matching.Count == 0 )
        {
            return null;
        }

        var nestedOptionsType = typeof( LoadConfigurationOptions<> ).MakeGenericType( nestedType );
        var nestedOptions = Activator.CreateInstance( nestedOptionsType )!;
        var registerMethod = nestedOptionsType.GetMethod(
            "RegisterConverter",
            BindingFlags.Instance | BindingFlags.NonPublic )!;

        foreach( var (configurationPath, converter) in matching )
        {
            var nestedProperty = configurationPath[ prefix.Length.. ];
            registerMethod.Invoke( nestedOptions, [ nestedProperty, converter.valueType, converter.converter ] );
        }

        return nestedOptions;
    }
}