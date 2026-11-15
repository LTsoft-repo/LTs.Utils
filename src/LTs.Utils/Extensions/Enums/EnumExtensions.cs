namespace LTs.Utils.Extensions.Enums;

/// <summary>
///     Extension methods for <see cref="Enum" /> values and names.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Parses a string into an enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="ignoreCase">Whether to ignore case when parsing.</param>
    /// <returns>The parsed enum value.</returns>
    public static T ToEnum<T>( this string? value, bool ignoreCase = true )
        where T : struct, Enum
    {
        if( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( "Enum value cannot be null or empty.", nameof( value ) );
        }

        return Enum.Parse<T>( value, ignoreCase );
    }

    /// <summary>
    ///     Gets the enum member name string for a value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The member name.</returns>
    public static string ToValueString<T>( this T value )
        where T : struct, Enum
        => Enum.GetName( typeof( T ), value )
           ?? value.ToString();
}
