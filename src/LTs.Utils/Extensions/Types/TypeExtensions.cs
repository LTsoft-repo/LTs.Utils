using System.Reflection;

namespace LTs.Utils.Extensions.Types;

/// <summary>
///     Extensions for <see cref="Type" />.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Check if the type is a Basic Type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is a Basic Type; otherwise, <c>false</c>.</returns>
    public static bool IsBasicType( this Type type ) => type == typeof( string ) ||
                                                        type == typeof( int ) ||
                                                        type == typeof( double ) ||
                                                        type == typeof( bool ) ||
                                                        type == typeof( char ) ||
                                                        type == typeof( decimal ) ||
                                                        type == typeof( float ) ||
                                                        type == typeof( byte ) ||
                                                        type == typeof( short ) ||
                                                        type == typeof( long ) ||
                                                        type == typeof( sbyte ) ||
                                                        type == typeof( ushort ) ||
                                                        type == typeof( uint ) ||
                                                        type == typeof( ulong ) ||
                                                        type.IsEnum;

    /// <summary>
    ///     Check if the type is an Anonymous Type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if the type is an Anonymous Type; otherwise, <c>false</c>.</returns>
    public static bool IsAnonymousType( this Type type )
    {
        var typeName = type.Name;

        return typeName.StartsWith( "<>" ) && typeName.Contains( "AnonymousType" );
    }

    /// <summary>
    ///     Check if the object is the Default value.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <returns><c>true</c> if the object is the Default value; otherwise, <c>false</c>.</returns>
    public static bool IsDefault<T>( this T? obj )
        => obj == null || AreAllFieldsAndPropertiesDefault( obj );

    /// <summary>
    ///     Check if all the fields and properties of the object are the Default values.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns><c>true</c> if all the fields and properties of the object are the Default values; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static bool AreAllFieldsAndPropertiesDefault( object obj )
    {
        ArgumentNullException.ThrowIfNull( obj );

        var type = obj.GetType();

        if( type.IsBasicType() || type.IsArray )
        {
            return false;
        }

        // Anonymous types.
        if( type.IsAnonymousType() )
        {
            foreach( var property in type.GetProperties() )
            {
                if( property.GetValue( obj ) != default )
                {
                    return false;
                }
            }

            return true;
        }

        // Other Types.
        var defaultObject = Activator.CreateInstance( type );

        // Check fields
        foreach( var field in type.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) )
        {
            var fieldValueObj = field.GetValue( obj );
            var fieldValueNewObj = field.GetValue( defaultObject );

            if( !AreValuesEqual( fieldValueObj, fieldValueNewObj ) )
            {
                return false;
            }
        }

        // Check properties
        foreach( var prop in type.GetProperties( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) )
        {
            var propValueObj = prop.GetValue( obj );
            var propValueNewObj = prop.GetValue( defaultObject );

            if( !AreValuesEqual( propValueObj, propValueNewObj ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Check if two values are equal.
    /// </summary>
    /// <param name="obj1">Object 1.</param>
    /// <param name="obj2">Object 2.</param>
    /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
    private static bool AreValuesEqual( object? obj1, object? obj2 )
        => obj1 is null && obj2 is null
           ||
           obj1 is not null && obj2 is not null && obj1.Equals( obj2 );
}