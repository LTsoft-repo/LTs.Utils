namespace LTs.Utils.Time.Abstractions;

// ToDo: Move to LTs.Utils solution.

/// <summary>
///     Supplies the current date and time.
/// </summary>
public interface IDateTimeOffsetProvider
{
    /// <summary>
    ///     Gets the current date and time.
    /// </summary>
    DateTimeOffset Now { get; }

    /// <summary>
    ///     Gets the current date and time in Coordinated Universal Time (UTC).
    /// </summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>
    ///     Gets the local time zone identifier.
    /// </summary>
    string TimeZoneId { get; }

    /// <summary>
    ///     Gets the local time zone offset in hours from Coordinated Universal Time (UTC).
    /// </summary>
    TimeSpan LocalTimeZoneOffset { get; }
}