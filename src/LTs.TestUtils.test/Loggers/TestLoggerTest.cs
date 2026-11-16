using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Loggers;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.test.Loggers;

public class TestLoggerTest
{
    private readonly ITestOutputHelper testOutput;

    // ReSharper disable once ConvertToPrimaryConstructor
    public TestLoggerTest( ITestOutputHelper testOutput )
        => this.testOutput = testOutput;

    #region TestLogger
    [ Fact ]
    public void TestLogger_ShouldLog()
    {
        // Arrange
        var logger = new TestLogger( testOutput );

        // Act
        logger.Log( LogLevel.Information,
                    new EventId( 1 ),
                    "Test log",
                    null,
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    // ReSharper disable once UnusedParameter.Local
                    ( state, exception ) => "{state}" + exception == null ? "" : Environment.NewLine + exception );

        // Assert
        testOutput.Should().HaveMessage().Containing( "Test log" );
    }
    #endregion

    #region TestLoggerT
    [ Fact ]
    public void TestLoggerT_ShouldLog()
    {
        // Arrange
        var logger = new TestLogger<TestLoggerTest>( testOutput );

        // Act
        logger.Log( LogLevel.Information,
                    new EventId( 1 ),
                    "Test log",
                    null,
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    // ReSharper disable once UnusedParameter.Local
                    ( state, exception ) => "{state}" + exception == null ? "" : Environment.NewLine + exception );

        // Assert
        testOutput.Should().HaveMessage().Containing( "Test log" );

        testOutput.Should().HaveMessage().Containing( typeof( TestLoggerTest ).FullName! );
    }
    #endregion

    #region LogLevelTags
    [ Theory ]
    [ InlineData( LogLevel.Trace, "TRC" ) ]
    [ InlineData( LogLevel.Debug, "DBG" ) ]
    [ InlineData( LogLevel.Information, "INF" ) ]
    [ InlineData( LogLevel.Warning, "WRN" ) ]
    [ InlineData( LogLevel.Error, "ERR" ) ]
    [ InlineData( LogLevel.Critical, "CRT" ) ]
    [ InlineData( LogLevel.None, "UNK" ) ]
    public void TestLogger_WritesLogLevelTag( LogLevel logLevel, string expectedTag )
    {
        // Arrange
        var logger = new TestLogger( testOutput );

        // Act
        logger.Log( logLevel, new EventId( 1 ), "Tagged message", null, ( state, _ ) => state?.ToString() ?? "" );

        // Assert
        var act = () => testOutput.Should().HaveMessage( logLevel ).Containing( "Tagged message" );
        act.Should().NotThrow();

        testOutput.Should().HaveMessage().Containing( $"[{expectedTag}]" );
    }
    #endregion
}
