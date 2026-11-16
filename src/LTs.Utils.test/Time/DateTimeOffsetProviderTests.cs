using LTs.Utils.Time;

#pragma warning disable IDE0290

namespace LTs.Utils.test.Time;

public class DateTimeOffsetProviderTests : BaseTest
{
    public DateTimeOffsetProviderTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Now
    [ Fact ]
    public void Now_ReturnsCurrentSystemTime()
    {
        // Arrange
        var before = DateTimeOffset.Now;
        var provider = new DateTimeOffsetProvider();

        // Act
        var now = provider.Now;
        var after = DateTimeOffset.Now;

        // Assert
        now.Should().BeOnOrAfter( before );

        now.Should().BeOnOrBefore( after );
    }
    #endregion

    #region UtcNow
    [ Fact ]
    public void UtcNow_ReturnsCurrentSystemUtcTime()
    {
        // Arrange
        var before = DateTimeOffset.UtcNow;
        var provider = new DateTimeOffsetProvider();

        // Act
        var utcNow = provider.UtcNow;
        var after = DateTimeOffset.UtcNow;

        // Assert
        utcNow.Should().BeOnOrAfter( before );

        utcNow.Should().BeOnOrBefore( after );
    }
    #endregion

    #region TimeZoneId
    [ Fact ]
    public void TimeZoneId_ReturnsLocalTimeZoneId()
    {
        // Arrange
        var provider = new DateTimeOffsetProvider();
        var expectedTimeZoneId = TimeZoneInfo.Local.Id;

        // Act
        var timeZoneId = provider.TimeZoneId;

        // Assert
        timeZoneId.Should().Be( expectedTimeZoneId );
    }
    #endregion

    #region LocalTimeZoneOffset
    [ Fact ]
    public void LocalTimeZoneOffset_ReturnsLocalUtcOffsetForCurrentTime()
    {
        // Arrange
        var provider = new DateTimeOffsetProvider();
        var expectedOffset = TimeZoneInfo.Local.GetUtcOffset( DateTimeOffset.Now );

        // Act
        var localTimeZoneOffset = provider.LocalTimeZoneOffset;

        // Assert
        localTimeZoneOffset.Should().Be( expectedOffset );
    }
    #endregion
}