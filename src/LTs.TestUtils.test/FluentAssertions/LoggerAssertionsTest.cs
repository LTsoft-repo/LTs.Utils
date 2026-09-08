using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.test.FluentAssertions.Infrastructure;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;
using Xunit.Sdk;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.test.FluentAssertions;

public class LoggerAssertionsTest : BaseTest
{
    public LoggerAssertionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Should
    [ Fact ]
    public void Should_WithInMemoryLogger_HaveMessage_Containing_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().HaveMessage().Containing( "Application started." );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void Should_WithInMemoryLoggerT_HaveMessage_Containing_Succeeds()
    {
        // Arrange
        ILogger<LoggerAssertionsTest> logger = new InMemoryLogger<LoggerAssertionsTest>();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().HaveMessage().Containing( "Application started." );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void Should_WithTestLoggerT_NotHaveMessage_AtLogLevel_Containing_Succeeds()
    {
        // Arrange
        ILogger<LoggerAssertionsTest> logger = new TestLogger<LoggerAssertionsTest>( TestOutput );
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().NotHaveMessage( LogLevel.Error ).Containing( string.Empty, because: "no error should occur" );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void Should_WithUnsupportedLogger_Throws()
    {
        // Arrange
        ILogger logger = new UnsupportedLogger();

        // Act
        var act = () => logger.Should().HaveMessage().Containing( "Application started." );

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
           .WithMessage( "Cannot assert log messages on *UnsupportedLogger*. Use an ILogger implementation that derives from InMemoryLogger, such as InMemoryLogger<T> or TestLogger<T>." );
    }
    #endregion

    #region HaveMessage
    [ Fact ]
    public void HaveMessage_Containing_WhenMessageIsPresent_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().HaveMessage().Containing( "Application started." );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void HaveMessage_Containing_WhenMessageIsMissing_Throws()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Something else." );

        // Act
        var act = () => logger.Should().HaveMessage().Containing( "Application started." );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "Expected logger to have a message containing \"Application started.\"*, but no such message was found." );
    }

    [ Fact ]
    public void HaveMessage_AtLogLevel_Containing_WhenMessageIsPresent_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().HaveMessage( LogLevel.Information ).Containing( "Application started." );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region NotHaveMessage
    [ Fact ]
    public void NotHaveMessage_Containing_WhenMessageIsAbsent_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().NotHaveMessage().Containing( "Application failed." );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotHaveMessage_Containing_WhenMessageIsPresent_Throws()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().NotHaveMessage().Containing( "Application started." );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "Expected logger not to have a message containing \"Application started.\"*, but such a message was found." );
    }

    [ Fact ]
    public void NotHaveMessage_AtLogLevel_Containing_WhenLogLevelIsAbsent_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogInformation( "Application started." );

        // Act
        var act = () => logger.Should().NotHaveMessage( LogLevel.Error ).Containing( string.Empty, because: "no error should occur" );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotHaveMessage_AtLogLevel_Containing_WhenLogLevelIsPresentWithoutSubstring_Throws()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogError( "Application failed." );

        // Act
        var act = () => logger.Should().NotHaveMessage( LogLevel.Error ).Containing( string.Empty );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "*Expected logger not to have a message at level*Error*, but such a message was found." );
    }

    [ Fact ]
    public void NotHaveMessage_AtLogLevel_Containing_WhenLogLevelAndSubstringMatch_Throws()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogError( "Application failed." );

        // Act
        var act = () => logger.Should().NotHaveMessage( LogLevel.Error ).Containing( "application failed" );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "*Expected logger not to have a message at level*Error*application failed*, but such a message was found." );
    }

    [ Fact ]
    public void NotHaveMessage_AtLogLevel_Containing_WhenLogLevelMatchesButSubstringDoesNot_Succeeds()
    {
        // Arrange
        ILogger logger = new InMemoryLogger();
        logger.LogError( "Application failed." );

        // Act
        var act = () => logger.Should().NotHaveMessage( LogLevel.Error ).Containing( "timeout" );

        // Assert
        act.Should().NotThrow();
    }
    #endregion
}
