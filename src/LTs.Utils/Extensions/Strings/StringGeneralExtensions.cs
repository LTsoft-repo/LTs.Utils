using System.Text.RegularExpressions;

namespace LTs.Utils.Extensions.Strings;

/// <summary>
///     General string extensions.
/// </summary>
public static class StringGeneralExtensions
{
    /// <summary>
    ///     Removes the last occurrence of <paramref name="remove" /> from the end of the string.
    /// </summary>
    /// <param name="text">The source text.</param>
    /// <param name="remove">The text to remove from the end of the source text.</param>
    /// <returns>The source text with the last occurrence of the specified text removed from the end.</returns>
    public static string TrimEnd( this string? text, string? remove )
    {
        if( text == null ||
            remove == null ||
            !text.EndsWith( remove ) )
        {
            return text ?? string.Empty;
        }

        // remove the textToRemove from the end of the string
        var result = text[ ..^remove.Length ];

        return result;
    }

    /// <summary>
    ///     Converts the first character of a string to lowercase.
    /// </summary>
    /// <param name="s"> The string to convert.</param>
    /// <returns></returns>
    public static string? LowercaseFirst( this string? s )
    {
        if( string.IsNullOrEmpty( s ) || char.IsLower( s[ 0 ] ) )
        {
            return s;
        }

        return char.ToLowerInvariant( s[ 0 ] ) + s[ 1.. ];
    }

    /// <summary>
    ///     Capitalizes the first letter of a string.
    /// </summary>
    /// <param name="str">The string to capitalize.</param>
    /// <returns>The capitalized string.</returns>
    public static string? Capitalize( this string? str )
    {
        if( string.IsNullOrEmpty( str ) )
        {
            return str;
        }

        return char.ToUpper( str[ 0 ] ) + str[ 1.. ];
    }

    /// <summary>
    ///     Searches for the first occurrence of a substring within a string within a specified range.
    /// </summary>
    /// <param name="str">The source string to search within.</param>
    /// <param name="search">The substring to search for.</param>
    /// <param name="start"> The starting index for the search. If null, defaults to 0.</param>
    /// <param name="end"> The ending index for the search. If null, defaults to the length of the string.</param>
    /// <returns> The index of the first occurrence of the substring, or <see langword="null" /> if not found.</returns>
    public static int? Find( this string str, string search, int? start, int? end )
    {
        if( string.IsNullOrEmpty( str ) ||
            string.IsNullOrEmpty( search ) )
        {
            return null;
        }

        var s = Math.Max( start ?? 0, 0 );
        var e = Math.Min( end ?? ( str.Length == 0 ? 0 : str.Length ), str.Length );

        var substring = str.Substring( s, e - s );
        var index = substring.IndexOf( search, 0, StringComparison.OrdinalIgnoreCase );

        return index != -1 ? s + index : null;
    }

    /// <summary>
    ///     Searches for the last occurrence of a substring within a string within a specified range.
    /// </summary>
    /// <param name="str">The source string to search within.</param>
    /// <param name="search">The substring to search for.</param>
    /// <param name="start"> The starting index for the search. If  <see langword="null" />, defaults to 0.</param>
    /// <param name="end"> The ending index for the search. If null, defaults to the length of the string.</param>
    /// <returns> The index of the last occurrence of the substring, or <see langword="null" /> if not found.</returns>
    public static int? FindLast( this string str, string search, int? start, int? end )
    {
        if( string.IsNullOrEmpty( str ) ||
            string.IsNullOrEmpty( search ) )
        {
            return null;
        }

        var s = Math.Max( start ?? 0, 0 );
        var e = Math.Min( end ?? ( str.Length == 0 ? 0 : str.Length ), str.Length );

        var substring = str.Substring( s, e - s );
        var index = substring.LastIndexOf( search, StringComparison.OrdinalIgnoreCase );

        return index != -1 ? s + index : null;
    }

    /// <summary>
    ///     Replaces all occurrences of a specified string in the current string with another specified string limiting the
    ///     number of replacements.
    /// </summary>
    /// <param name="str">The source string to search within.</param>
    /// <param name="replace">The substring to search for.</param>
    /// <param name="with">The substring to replace with.</param>
    /// <param name="count">
    ///     The maximum number of replacements to perform. If  <see langword="null" />, all occurrences will be
    ///     replaced.
    /// </param>
    /// <returns>The modified string with the specified replacements.</returns>
    public static string Replace( this string str, string replace, string with, int? count )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if( string.IsNullOrEmpty( str ) ||
            string.IsNullOrEmpty( replace ) ||
            with is null )
        {
            return str;
        }

        var pattern = Regex.Escape( replace );

        var result = count != null
                         ? new Regex( pattern ).Replace( str, with, count.GetValueOrDefault() )
                         : new Regex( pattern ).Replace( str, with );

        return result;
    }

    /// <summary>
    ///     Splits a string into an array of substrings based on a specified separator, with an optional limit on the number of
    ///     splits.
    /// </summary>
    /// <param name="str">The source string to search within.</param>
    /// <param name="separator">The substring to search for as a separator.</param>
    /// <param name="count">The maximum number of splits to perform. If <see langword="null" />, all occurrences will be split.</param>
    /// <returns>An array of substrings resulting from the split operation.</returns>
    public static string[] Split( this string str, string separator, int? count )
    {
        ArgumentNullException.ThrowIfNull( str );

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if( string.IsNullOrEmpty( str ) ||
            separator is null )
        {
            return new[] { str };
        }

        if( string.IsNullOrEmpty( str ) ||
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            separator is null )
        {
            return new[] { str };
        }

        if( separator.Length == 0 )
        {
            var position = count ?? str.Length;
            var tail = str[ position.. ];
            var head = str[ ..position ];

            var heads = head.ToCharArray().Select( c => new string( new[] { c } ) );
            var remainders = tail.Length > 0 ? new[] { tail } : Enumerable.Empty<string>();

            return heads.Concat( remainders ).ToArray();
        }

        var split = count != null
                        ? str.Split( new[] { separator }, count.GetValueOrDefault() + 1, StringSplitOptions.None )
                        : str.Split( new[] { separator }, StringSplitOptions.None );

        return split;
    }
}