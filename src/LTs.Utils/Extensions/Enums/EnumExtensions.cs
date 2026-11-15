using System.Text.RegularExpressions;

namespace LTs.Utils.Extensions.Enums;

/// <summary>
///     Extension methods for <see cref="Enum" /> values and names.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Parses a enum value string or friendly name into an enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to parse.</param>
    /// <param name="ignoreCase">Whether to ignore case when parsing.</param>
    /// <returns>The parsed enum value.</returns>
    public static T ToEnum<T>( this string? value, bool ignoreCase = true )
        where T : struct, Enum
    {
        var enumName = value?.Replace( " ", "" );

        if( string.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( "Enum value cannot be null or empty.", nameof( value ) );
        }

        return Enum.TryParse<T>( enumName, ignoreCase, out var result )
                   ? result
                   : throw new ArgumentException( $"Invalid {nameof( T )}: {value}" );
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

    /// <summary>
    ///     Gets a friendly name for an enum value by inserting spaces before capital letters and between letters and digits.
    /// </summary>
    /// <param name="enumValue">The enum value.</param>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>The friendly name.</returns>
    public static string ToFriendlyName<T>( this T enumValue )
        where T : struct, Enum
    {
        var name = Regex.Replace( enumValue.ToString(), "(?<!^)([A-Z])", " $1" );

        return Regex.Replace( name, @"(?<=[a-zA-Z])(?=\d)|(?<=\d)(?=[a-zA-Z])", " " );
    }
}