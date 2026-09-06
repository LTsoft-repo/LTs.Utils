using System.Reflection;

namespace LTs.Utils.Reflection;

/// <summary>
///     Extensions for <c>?ConfigurationEntity</c> classes />.
/// </summary>
public static class ReflectionPropertiesExtensions
{
    /// <summary>
    ///     Copy the properties from <paramref name="source" /> to <paramref name="destination" />.
    /// </summary>
    /// <param name="destination">Destination object.</param>
    /// <param name="source">Source object.</param>
    /// <param name="ignoredSourceProperties">List of source property names to ignore.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static void CopyPropertiesFrom( this object destination,
                                           object source,
                                           string[] ignoredSourceProperties )
    {
        ArgumentNullException.ThrowIfNull( destination );
        ArgumentNullException.ThrowIfNull( source );

        foreach( var entityProp in source.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
        {
            if( ignoredSourceProperties.Contains( entityProp.Name ) )
            {
                continue;
            }

            var prop = destination.GetProperty( entityProp.Name );

            if( prop is null )
            {
                continue;
            }

            var value = entityProp.GetValue( source );
            prop.SetValue( destination, value );
        }
    }

    /// <summary>
    ///     Gets a public instance property by name.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static PropertyInfo? GetProperty( this object obj, string propertyName )
    {
        ArgumentNullException.ThrowIfNull( obj );

        return obj.GetType().GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
    }

    /// <summary>
    ///     Sets a public instance property by name.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <returns>
    ///     <see langword="true" /> if the property was found and set;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public static bool SetProperty( this object obj, string propertyName, object? value )
    {
        ArgumentNullException.ThrowIfNull( obj );

        var prop = obj.GetProperty( propertyName );

        if( prop is null )
        {
            return false;
        }

        if( !prop.PropertyType.IsAssignableFrom( value?.GetType() ?? typeof( object ) ) )
        {
            return false;
        }

        try
        {
            prop.SetValue( obj, value );
        }
        catch( Exception )
        {
            return false;
        }

        return true;
    }
}