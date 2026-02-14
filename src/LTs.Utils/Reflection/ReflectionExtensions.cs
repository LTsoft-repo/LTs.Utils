using System.Reflection;

namespace LTs.Utils.Reflection;

/// <summary>
///     Reflection extensions.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    ///     Gets the value of a field. Whether it is public, protected, internal or private.
    /// </summary>
    /// <typeparam name="T">The type of the field.</typeparam>
    /// <param name="obj">The object to get the field value from.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <returns>The value of the field.</returns>
    /// <exception cref="MissingFieldException"></exception>
    public static T? GetFieldValue<T>( this object obj, string fieldName )
    {
        ArgumentNullException.ThrowIfNull( obj, nameof( obj ) );
        ArgumentNullException.ThrowIfNull( fieldName, nameof( fieldName ) );

        var field = obj.GetField( fieldName );

        if( field == null )
        {
            throw new MissingFieldException( $"Field not found. (Field '{fieldName}')" );
        }

        return (T?)field.GetValue( obj );
    }

    /// <summary>
    ///     Gets the value of a property. Whether it is public, protected, internal or private.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="obj">The object to get the property value from.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>The value of the property.</returns>
    public static T? GetPropertyValue<T>( this object obj, string propertyName )
    {
        ArgumentNullException.ThrowIfNull( obj, nameof( obj ) );
        ArgumentNullException.ThrowIfNull( propertyName, nameof( propertyName ) );

        var property = obj.GetPropertyEvenPrivate( propertyName );

        if( property == null )
        {
            throw new MissingFieldException( $"Property not found. (Property '{propertyName}')" );
        }

        return (T?)property.GetValue( obj );
    }

    /// <summary>
    ///     Gets the value of a field/property. Whether it is public, protected, internal or private.
    /// </summary>
    /// <typeparam name="T">The type of the field/property.</typeparam>
    /// <param name="obj">The object to get the field/property value from.</param>
    /// <param name="dataMemberName">Name of the field/property.</param>
    /// <returns>The value of the field/property.</returns>
    public static T? GetDataMemberValue<T>( this object obj, string dataMemberName )
    {
        ArgumentNullException.ThrowIfNull( obj, nameof( obj ) );
        ArgumentNullException.ThrowIfNull( dataMemberName, nameof( dataMemberName ) );

        var property = obj.GetPropertyEvenPrivate( dataMemberName );

        if( property == null )
        {
            var field = obj.GetField( dataMemberName );

            if( field == null )
            {
                throw new MissingFieldException( $"Data member not found. (Data member '{dataMemberName}')" );
            }

            return (T?)field.GetValue( obj );
        }

        return (T?)property.GetValue( obj );
    }

    private static FieldInfo? GetField( this object obj, string fieldName )
        => obj.GetType().GetField( fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

    private static PropertyInfo? GetPropertyEvenPrivate( this object obj, string propertyName )
        => obj.GetType().GetProperty( propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
}