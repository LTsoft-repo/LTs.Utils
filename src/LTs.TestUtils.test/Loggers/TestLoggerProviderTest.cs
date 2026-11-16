using LTs.TestUtils;
using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.test.Infrastructure;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;
using Xunit.Sdk;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.test.Loggers;

public class TestLoggerProviderTest : BaseTest
{
    public TestLoggerProviderTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region UseTestLogger
    [ Fact ]
    public void UseTestLogger_WhenLogging_WritesToMessageSinkAndTestOutput()
    {
        // Arrange
        var messageSink = new CapturingMessageSink();
        var provider = new TestLoggerProvider( messageSink );

        // Act
        using( provider.UseTestLogger( TestOutput ) )
        {
            provider.LogInformation( "Application started." );
        }

        // Assert
        messageSink.Messages.Count.Should().Be( 1 );
        messageSink.Messages[ 0 ].Should().BeOfType<DiagnosticMessage>();
        messageSink.Messages[ 0 ].ToString().Should().Be( "Application started." );

        provider.CoreToolsLogs.Should().ContainExactlyEquivalent( [ "Application started." ] );

        var act = () => TestOutput.Should().HaveMessage().Containing( "Application started." );
        act.Should().NotThrow();
    }

    [ Fact ]
    public void UseTestLogger_AfterDispose_DoesNotWriteToTestOutput()
    {
        // Arrange
        var messageSink = new CapturingMessageSink();
        var provider = new TestLoggerProvider( messageSink );

        using( provider.UseTestLogger( TestOutput ) )
        {
            provider.LogInformation( "Before dispose." );
        }

        // Act
        provider.LogInformation( "After dispose." );

        // Assert
        provider.CoreToolsLogs.Should().ContainExactlyEquivalent( [ "Before dispose.", "After dispose." ] );

        var act = () => TestOutput.Should().HaveMessage().Containing( "After dispose." );
        act.Should().ThrowExactly<XunitException>();
    }
    #endregion

    #region CreateLogger
    [ Fact ]
    public void CreateLogger_ReturnsProviderInstance()
    {
        // Arrange
        var messageSink = new CapturingMessageSink();
        var provider = new TestLoggerProvider( messageSink );

        // Act
        var resolvedLogger = provider.CreateLogger( "Category" );

        // Assert
        var sameInstance = ReferenceEquals( resolvedLogger, provider );
        sameInstance.Should().BeTrue();
    }
    #endregion

    #region Log
    [ Fact ]
    public void Log_WhenFormatterThrows_DoesNotThrow()
    {
        // Arrange
        var messageSink = new CapturingMessageSink();
        var provider = new TestLoggerProvider( messageSink );

        // Act
        var act = () => provider.Log( LogLevel.Information,
                                      new EventId( 1 ),
                                      "state",
                                      null,
                                      ( _, _ ) => throw new InvalidOperationException( "Formatter failed." ) );

        // Assert
        act.Should().NotThrow();
        messageSink.Messages.Should().BeEmpty();
        provider.CoreToolsLogs.Should().BeEmpty();
    }
    #endregion

    #region IsEnabled
    [ Theory ]
    [ InlineData( LogLevel.Trace ) ]
    [ InlineData( LogLevel.Debug ) ]
    [ InlineData( LogLevel.Information ) ]
    [ InlineData( LogLevel.Warning ) ]
    [ InlineData( LogLevel.Error ) ]
    [ InlineData( LogLevel.Critical ) ]
    [ InlineData( LogLevel.None ) ]
    public void IsEnabled_ReturnsTrue( LogLevel logLevel )
    {
        // Arrange
        var provider = new TestLoggerProvider( new CapturingMessageSink() );

        // Act
        var enabled = provider.IsEnabled( logLevel );

        // Assert
        enabled.Should().BeTrue();
    }
    #endregion

    #region BeginScope
    [ Fact ]
    public void BeginScope_ReturnsNull()
    {
        // Arrange
        var provider = new TestLoggerProvider( new CapturingMessageSink() );

        // Act
        var scope = provider.BeginScope( "scope" );

        // Assert
        scope.Should().BeNull();
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_DoesNotThrow()
    {
        // Arrange
        var provider = new TestLoggerProvider( new CapturingMessageSink() );

        // Act
        var act = () => provider.Dispose();

        // Assert
        act.Should().NotThrow();
    }
    #endregion
}