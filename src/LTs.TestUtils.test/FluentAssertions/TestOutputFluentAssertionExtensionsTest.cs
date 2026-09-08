using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;
using Xunit.Sdk;

#pragma warning disable IDE0290 // Primary constructor should be used

namespace LTs.TestUtils.test.FluentAssertions;

public class TestOutputFluentAssertionExtensionsTest : BaseTest
{
    public TestOutputFluentAssertionExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region HaveMessage
    [ Fact ]
    public void HaveMessage_Containing_WhenMessageIsPresent_Succeeds()
    {
        // Arrange
        TestOutput.WriteLine( "Hello from test" );

        // Act
        var act = () => TestOutput.Should().HaveMessage().Containing( "Hello from test" );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void HaveMessage_Containing_WhenMessageIsMissing_Throws()
    {
        // Arrange
        TestOutput.WriteLine( "Something else" );

        // Act
        var act = () => TestOutput.Should().HaveMessage().Containing( "missing message" );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "Expected test output to have a message containing \"missing message\"*, but found:*" );
    }

    [ Fact ]
    public void HaveMessage_Containing_WithTestLogger_Succeeds()
    {
        // Arrange
        var logger = new TestLogger( TestOutput );

        // Act
        logger.LogInformation( "Application started." );

        // Assert
        var act = () => TestOutput.Should().HaveMessage().Containing( "Application started." );

        act.Should().NotThrow();
    }

    [ Fact ]
    public void HaveMessage_AtLogLevel_Containing_WhenMessageIsPresent_Succeeds()
    {
        // Arrange
        var logger = new TestLogger( TestOutput );
        logger.LogInformation( "Application started." );

        // Act
        var act = () => TestOutput.Should().HaveMessage( LogLevel.Information ).Containing( "Application started." );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region NotHaveMessage
    [ Fact ]
    public void NotHaveMessage_Containing_WhenMessageIsAbsent_Succeeds()
    {
        // Arrange
        TestOutput.WriteLine( "Application started." );

        // Act
        var act = () => TestOutput.Should().NotHaveMessage().Containing( "Application failed." );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotHaveMessage_Containing_WhenMessageIsPresent_Throws()
    {
        // Arrange
        TestOutput.WriteLine( "Application started." );

        // Act
        var act = () => TestOutput.Should().NotHaveMessage().Containing( "Application started." );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "Expected test output not to have a message containing \"Application started.\"*, but found:*" );
    }
    #endregion
}
