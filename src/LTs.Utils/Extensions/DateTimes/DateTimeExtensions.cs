using System.Globalization;
using Newtonsoft.Json;

namespace LTs.Utils.Extensions.DateTimes;

/// <summary>
///     Extension methods for <see cref="DateTime" />.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    ///     Gets the next <paramref name="day" /> from the given <paramref name="date" />.
    /// </summary>
    /// <param name="date">The given date.</param>
    /// <param name="day">The day of the week to get.</param>
    /// <returns>The <see cref="DateTime" /> for the next <paramref name="day" />.</returns>
    public static DateTime GetNext( this DateTime date, DayOfWeek day )
    {
        var daysToAdd = ( (int)day - (int)date.DayOfWeek + 7 ) % 7;

        if( daysToAdd == 0 )
        {
            daysToAdd = 7;
        }

        return date.AddDays( daysToAdd );
    }

    /// <summary>
    ///     Gets the offset of the given <paramref name="dateTimeOffset" /> from UTC.
    /// </summary>
    /// <param name="dateTimeOffset">DateTime to get the offset for.</param>
    /// <returns>The <see cref="TimeSpan" />  offset from UTC.</returns>
    public static TimeSpan GetOffset( this DateTimeOffset dateTimeOffset )
    {
        var offset = dateTimeOffset.Offset;

        return offset;
    }

    /// <summary>
    ///     Converts the value of the current <see cref="TimeSpan" /> to its signed string representation.
    /// </summary>
    /// <param name="timeSpan">Value to convert.</param>
    /// <param name="includePositiveSign">Defines whether to include a '+' sign for positive values.</param>
    /// <returns>The string representation of the current <see cref="TimeSpan" /> with a sign prefix.</returns>
    /// <exception cref="FormatException">The format specification is invalid.</exception>
    public static string ToStringSigned( this TimeSpan timeSpan, bool includePositiveSign = true )
        => timeSpan.ToStringSigned( null, includePositiveSign );

    /// <summary>
    ///     Converts the value of the current <see cref="TimeSpan" /> to its signed string representation by using the
    ///     specified format.
    /// </summary>
    /// <param name="timeSpan">Value to convert.</param>
    /// <param name="format">Format string.</param>
    /// <param name="includePositiveSign">Defines whether to include a '+' sign for positive values.</param>
    /// <returns>The string representation of the current <see cref="TimeSpan" /> with a sign prefix.</returns>
    /// <exception cref="FormatException">The format specification is invalid.</exception>
    public static string ToStringSigned( this TimeSpan timeSpan, string? format, bool includePositiveSign = true )
    {
        var sign = string.Empty;

        if( timeSpan < TimeSpan.Zero )
        {
            sign = "-";
        }
        else if( includePositiveSign )
        {
            sign = "+";
        }

        return sign + timeSpan.Duration().ToString( format, CultureInfo.InvariantCulture );
    }

    /// <summary>
    ///     Parses a <see cref="string" /> to <see cref="DateTime" />.
    /// </summary>
    /// <remarks>This is more robust than DateTimeOffset.Parse since it correctly converts the timezone.</remarks>
    /// <param name="value">Value to parse.</param>
    /// <returns>The parsed <see cref="DateTime" />.</returns>
    public static DateTimeOffset ParseAsDateTimeOffset( this string value )
    {
        try
        {
            return JsonConvert.DeserializeObject<DateTimeOffset>( $"\"{value}\"" );
        }

        catch( Exception ex )
        {
            if( ex is JsonReaderException or JsonSerializationException )
            {
                throw new InvalidOperationException( $"Could not convert '{value}' to DateTimeOffset." );
            }

            throw;
        }
    }

    /// <summary>
    ///     Parses a <see cref="string" /> to <see cref="DateTime" />.
    /// </summary>
    /// <remarks>This is more robust than DateTime.Parse since it correctly converts the timezone.</remarks>
    /// <param name="value">Value to parse.</param>
    /// <returns>The parsed <see cref="DateTime" />.</returns>
    public static DateTime ParseAsDateTime( this string value )
    {
        try
        {
            return JsonConvert.DeserializeObject<DateTime>( $"\"{value}\"" );
        }
        catch( Exception ex )
        {
            if( ex is JsonReaderException or JsonSerializationException )
            {
                throw new InvalidOperationException( "Could not convert string to DateTime." );
            }

            throw;
        }
    }
}