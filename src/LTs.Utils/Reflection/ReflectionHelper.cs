using System.Reflection;

namespace LTs.Utils.Reflection;

/// <summary>
///     Reflection helper to make it easier to create instances of objects.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    ///     Creates an instance of the specified type. Whether the constructor is public, protected, internal or private.
    /// </summary>
    /// <typeparam name="T">The type to create an instance of.</typeparam>
    /// <returns>An instance of the specified type. </returns>
    public static T CreateInstance<T>()
        => CreateInstance<T>( Array.Empty<object>() );

    /// <summary>
    ///     Creates an instance of the specified type. Whether the constructor is public, protected, internal or private.
    /// </summary>
    /// <typeparam name="T">The type to create an instance of.</typeparam>
    /// <param name="ctorParameters">The parameters to pass to the constructor. </param>
    /// <returns>An instance of the specified type. </returns>
    public static T CreateInstance<T>( params object[] ctorParameters )
    {
        var constructor = typeof( T ).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
            null,
            ctorParameters.Select( a => a.GetType() ).ToArray(),
            null );

        _ = constructor ?? throw new MissingMethodException( $"Constructor for {typeof( T ).Name} not found." );

        return (T)constructor.Invoke( ctorParameters );
    }
}