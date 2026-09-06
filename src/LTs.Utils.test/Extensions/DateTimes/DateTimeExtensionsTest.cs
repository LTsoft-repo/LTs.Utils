using LTs.Utils.Extensions.DateTimes;

namespace LTs.Utils.test.Extensions.DateTimes;

public class DateTimeExtensionsTest
{
    #region GetNext
    [ Theory ]
    [ InlineData( "2024-10-07", DayOfWeek.Sunday, "2024-10-13" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Monday, "2024-10-14" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Tuesday, "2024-10-08" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Wednesday, "2024-10-09" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Thursday, "2024-10-10" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Friday, "2024-10-11" ) ]
    [ InlineData( "2024-10-07", DayOfWeek.Saturday, "2024-10-12" ) ]
    [ InlineData( "2024-10-04", DayOfWeek.Monday, "2024-10-07" ) ]
    public void GetNext_Successes( string date, DayOfWeek day, string expected )
    {
        // Arrange
        var dateValue = DateTime.Parse( date );
        var expectedValue = DateTime.Parse( expected );

        // Act
        var act = dateValue.GetNext( day );

        // Assert
        act.Should().Be( expectedValue );
    }
    #endregion

    #region ToStringSigned
    [ Theory ]
    [ InlineData( 1, 5, 0, 0, "", true, "+1.05:00:00" ) ]
    [ InlineData( 1, 5, 0, 0, "", false, "1.05:00:00" ) ]
    [ InlineData( 1, 5, 0, 0, null, true, "+1.05:00:00" ) ]
    [ InlineData( 1, -3, 30, 0, "", true, "+21:30:00" ) ]
    [ InlineData( -1, -3, -30, 0, "", true, "-1.03:30:00" ) ]
    [ InlineData( -1, -3, -30, 0, "", false, "-1.03:30:00" ) ]
    [ InlineData( 0, -3, -30, 0, "", true, "-03:30:00" ) ]
    [ InlineData( 0, 0, 0, 0, "", true, "+00:00:00" ) ]
    [ InlineData( 0, 0, 0, 0, "", false, "00:00:00" ) ]
    [ InlineData( 0, -3, -30, 0, @"d\.hh\:mm\:ss", true, "-0.03:30:00" ) ]
    [ InlineData( 0, -3, -30, 0, @"hh\:mm", true, "-03:30" ) ]
    [ InlineData( 0, -13, -30, 0, @"hh\:mm", true, "-13:30" ) ]
    [ InlineData( 0, 25, -30, 0, @"hh\:mm", true, "+00:30" ) ]
    public void ToStringSigned_Successes( int days,
                                          int hours,
                                          int minutes,
                                          int seconds,
                                          string? format,
                                          bool includePositiveSign,
                                          string expectedResult )
    {
        // Arrange
        var timeSpan = new TimeSpan( days, hours, minutes, seconds );

        // Act
        var result = timeSpan.ToStringSigned( format, includePositiveSign );

        // Assert
        result.Should().Be( expectedResult );
    }
    #endregion

    #region GetOffset (DateTimeOffset)
    [ Theory ]
    [ InlineData( "2024-10-07T12:34:56Z", 0 ) ]
    [ InlineData( "2024-10-07T12:34:56+02:00", 2 ) ]
    [ InlineData( "2024-10-07T12:34:56-05:00", -5 ) ]
    [ InlineData( "2024-10-07T12:34:56", null ) ] // Local time
    public void GetOffset_DateTimeOffset_Successes( string dateTimeString, int? expectedOffsetMinutes )
    {
        // Arrange
        var dateTime = DateTimeOffset.Parse( dateTimeString );

        TimeSpan expectedOffset;

        if( expectedOffsetMinutes is null )
        {
            var localDatetime = DateTime.Parse( dateTimeString );
            localDatetime = localDatetime.ToLocalTime();
            localDatetime.Kind.Should().Be( DateTimeKind.Local );

            var localOffset = new DateTimeOffset( localDatetime );
            expectedOffset = localOffset.Offset;
        }
        else
        {
            expectedOffset = TimeSpan.FromHours( expectedOffsetMinutes.Value );
        }


        // Act
        var result = dateTime.GetOffset();

        // Assert
        result.Should().Be( expectedOffset );
    }
    #endregion

    #region ParseAsDateTimeOffset
    [ Theory ]
    [ InlineData( "2024-10-07T12:34:56Z", 2024, 10, 7, 12, 34, 56, 0, "00:00" ) ]
    [ InlineData( "2024-10-07T12:34:56+02:00", 2024, 10, 7, 12, 34, 56, 0, "02:00" ) ]
    [ InlineData( "2024-10-07T12:34:56-05:00", 2024, 10, 7, 12, 34, 56, 0, "-05:00" ) ]
    [ InlineData( "2024-10-07T12:34:56.789Z", 2024, 10, 7, 12, 34, 56, 789, "00:00" ) ]
    [ InlineData( "2024-10-07T12:34:56.789+02:00", 2024, 10, 7, 12, 34, 56, 789, "02:00" ) ]
    [ InlineData( "2024-10-07T12:34:56.789-05:00", 2024, 10, 7, 12, 34, 56, 789, "-05:00" ) ]
    [ InlineData( "2024-10-07T12:34:56.789", 2024, 10, 7, 12, 34, 56, 789, "local" ) ]
    public void ParseAsDateTimeOffset_Successes( string value,
                                                 int year,
                                                 int month,
                                                 int day,
                                                 int hour,
                                                 int minute,
                                                 int second,
                                                 int millisecond,
                                                 string offset )
    {
        // Arrange
        if( offset == "local" )
        {
            var localOffset = TimeZoneInfo.Local.GetUtcOffset( new DateTime( year, month, day, hour, minute, second ) );
            offset = localOffset.ToStringSigned( @"hh\:mm" );
        }

        var expected = new DateTimeOffset( year, month, day, hour, minute, second, millisecond, TimeSpan.Parse( offset ) );

        // Act
        var result = value.ParseAsDateTimeOffset();

        // Assert
        result.Should().Be( expected );
    }

    [ Theory ]
    [ InlineData( null ) ]
    [ InlineData( "" ) ]
    [ InlineData( "   " ) ]
    [ InlineData( "a" ) ]
    public void ParseAsDateTimeOffset_InvalidDate_Throws( string? value )
    {
        // Arrange

        // Act
        Action act = () => value!.ParseAsDateTimeOffset();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage( $"Could not convert '{value}' to DateTimeOffset." );
    }
    #endregion

    #region ParseAsDateTime
    [ Theory ]
    [ InlineData( "2024-10-07T12:34:56Z", 2024, 10, 7, 12, 34, 56, 0, DateTimeKind.Utc ) ]
    [ InlineData( "2024-10-07T12:34:56+02:00", 2024, 10, 7, 10, 34, 56, 0, DateTimeKind.Utc ) ]
    [ InlineData( "2024-10-07T12:34:56-05:00", 2024, 10, 7, 17, 34, 56, 0, DateTimeKind.Utc ) ]
    [ InlineData( "2024-10-07T12:34:56", 2024, 10, 7, 12, 34, 56, 0, DateTimeKind.Local ) ]
    [ InlineData( "2024-10-07T12:34:56.789Z", 2024, 10, 7, 12, 34, 56, 789, DateTimeKind.Utc ) ]
    [ InlineData( "2024-10-07T12:34:56.789+02:00", 2024, 10, 7, 10, 34, 56, 789, DateTimeKind.Utc ) ]
    [ InlineData( "2024-10-07T12:34:56.789-05:00", 2024, 10, 7, 17, 34, 56, 789, DateTimeKind.Utc ) ]
    public void ParseAsDateTime_Successes( string value,
                                           int year,
                                           int month,
                                           int day,
                                           int hour,
                                           int minute,
                                           int second,
                                           int millisecond,
                                           DateTimeKind kind )
    {
        // Arrange
        var expected = new DateTime( year, month, day, hour, minute, second, millisecond, kind );

        expected = expected.Kind != DateTimeKind.Utc
                       ? expected.ToUniversalTime()
                       : expected;

        // Act
        var result = value.ParseAsDateTime();

        // Assert
        result = result.Kind != DateTimeKind.Utc
                     ? result.ToUniversalTime()
                     : result;

        result.Should().Be( expected );
    }
    #endregion
}