using LTs.Utils.Time.Abstractions;

namespace LTs.Utils.Time;

// ToDo: Move to LTs.Utils solution.

/// <summary>
///     Returns the current system date, time, and local time zone.
/// </summary>
public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    /// <inheritdoc />
    public DateTimeOffset Now => DateTimeOffset.Now;

    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public string TimeZoneId => TimeZoneInfo.Local.Id;

    /// <inheritdoc />
    public TimeSpan LocalTimeZoneOffset => TimeZoneInfo.Local.GetUtcOffset( DateTimeOffset.Now );
}