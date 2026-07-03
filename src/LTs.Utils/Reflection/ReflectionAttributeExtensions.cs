using System.Reflection;

namespace LTs.Utils.Reflection;

/// <summary>
///     Extensions for reflection related to attributes.
/// </summary>
public static class ReflectionAttributeExtensions
{
    /// <summary>
    ///     Determines whether the specified type has an attribute of the specified type
    ///     <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to check for. Must derive from <see cref="Attribute" />.</typeparam>
    /// <param name="type">The <see cref="Type" /> for the class to inspect.</param>
    /// <returns>
    ///     <see langword="true" /> if the class has an attribute of type <typeparamref name="T" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasAttribute<T>( this Type type )
        where T : Attribute
        => type.HasAttribute( typeof( T ) );

    /// <summary>
    ///     Determines whether the specified type has an attribute of the specified type
    ///     <paramref name="attributeType" />.
    /// </summary>
    /// <param name="type">The <see cref="Type" /> for the class to inspect.</param>
    /// <param name="attributeType">
    ///     The <see cref="Type" /> of the attribute to check for. Must derive from <see cref="Attribute" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the class has an attribute of type <paramref name="attributeType" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    [ UsedImplicitly ]
    public static bool HasAttribute( this Type type, Type attributeType )
    {
        // Checks that attributeType is a subclass of Attribute.
        ArgumentNullException.ThrowIfNull( type );
        ArgumentNullException.ThrowIfNull( attributeType );

        if( !typeof( Attribute ).IsAssignableFrom( attributeType ) )
        {
            throw new ArgumentException( "attributeType must be a subclass of Attribute", nameof( attributeType ) );
        }

        // Checks if the attribute is defined on the type.
        return type.GetCustomAttributes( false ).Any( a => a.GetType() == attributeType );
    }

    /// <summary>
    ///     Determines whether the specified property has an attribute of the specified type
    ///     <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to check for. Must derive from <see cref="Attribute" />.</typeparam>
    /// <param name="propertyInfo">The <see cref="PropertyInfo" /> for the property to inspect.</param>
    /// <returns>
    ///     <see langword="true" /> if the property has an attribute of type <typeparamref name="T" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasAttribute<T>( this PropertyInfo propertyInfo )
        where T : Attribute
        => propertyInfo.HasAttribute( typeof( T ) );

    /// <summary>
    ///     Determines whether the specified property has an attribute of the specified type
    ///     <paramref name="attributeType" />.
    /// </summary>
    /// <param name="propertyInfo">The <see cref="PropertyInfo" /> for the property to inspect.</param>
    /// <param name="attributeType">
    ///     The <see cref="Type" /> of the attribute to check for. Must derive from <see cref="Attribute" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the property has an attribute of type <paramref name="attributeType" />;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    [ UsedImplicitly ]
    public static bool HasAttribute( this PropertyInfo propertyInfo, Type attributeType )
    {
        // Checks that attributeType is a subclass of Attribute.
        ArgumentNullException.ThrowIfNull( propertyInfo );
        ArgumentNullException.ThrowIfNull( attributeType );

        if( !typeof( Attribute ).IsAssignableFrom( attributeType ) )
        {
            throw new ArgumentException( "attributeType must be a subclass of Attribute", nameof( attributeType ) );
        }

        // Checks if the attribute is defined for the property.
        return propertyInfo.GetCustomAttributes( false ).Any( a => a.GetType() == attributeType );
    }

    /// <summary>
    ///     Finds the properties that have an attribute of the specified <paramref name="type" />
    /// </summary>
    /// <typeparam name="T">The type of the attribute to check for. Must derive from <see cref="Attribute" />.</typeparam>
    /// <param name="type">The <see cref="Type" /> for the class to inspect.</param>
    /// <returns>The <see cref="IEnumerable{PropertyInfo}" /> of properties that have the attribute.</returns>
    public static IEnumerable<PropertyInfo> FindPropertiesWithAttribute<T>( this Type type )
        where T : Attribute
        => type.FindPropertiesWithAttribute( typeof( T ) );

    /// <summary>
    ///     Finds the properties that have an attribute of the specified <paramref name="type" />
    /// </summary>
    /// <param name="type">The <see cref="Type" /> for the class to inspect.</param>
    /// <param name="attributeType">
    ///     The <see cref="Type" /> of the attribute to check for. Must derive from <see cref="Attribute" />.
    /// </param>
    /// <returns>The <see cref="IEnumerable{PropertyInfo}" /> of properties that have the attribute.</returns>
    [ UsedImplicitly ]
    public static IEnumerable<PropertyInfo> FindPropertiesWithAttribute( this Type type, Type attributeType )
        => type.GetProperties()
               .Where( p => p.HasAttribute( attributeType ) );
}