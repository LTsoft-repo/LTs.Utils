using System.Linq.Expressions;
using LTs.Utils.Reflection;

namespace LTs.Configurations.Configurations;

/// <summary>
///     Options for customizing how individual properties are loaded by
///     <see cref="TypedConfigurationLoader" />.
/// </summary>
/// <typeparam name="T">The configuration type.</typeparam>
public sealed class LoadConfigurationOptions<T>
    where T : notnull
{
    private readonly Dictionary<string, Func<string?, object?>> converters = new( StringComparer.OrdinalIgnoreCase );

    /// <summary>
    ///     Registers a custom converter for a configuration property.
    /// </summary>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="property">An expression that identifies the property.</param>
    /// <param name="convert">Converts the configuration value for the property.</param>
    /// <returns>The options instance.</returns>
    public LoadConfigurationOptions<T> SetValue<TProperty>( Expression<Func<T, TProperty>> property,
                                                          Func<string?, TProperty> convert )
    {
        ArgumentNullException.ThrowIfNull( property );
        ArgumentNullException.ThrowIfNull( convert );

        var propertyName = property.GetPropertyName();

        if( converters.ContainsKey( propertyName ) )
        {
            throw new ArgumentException(
                $"SetValue was already configured for property '{propertyName}'.",
                nameof( property ) );
        }

        converters[ propertyName ] = value => convert( value );

        return this;
    }

    internal bool TryGetConverter( string propertyName, out Func<string?, object?> converter )
        => converters.TryGetValue( propertyName, out converter! );
}
