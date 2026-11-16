using JetBrains.Annotations;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.test.Infrastructure;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.test.Loggers;

public class InMemoryLoggerTest
{
    #region InMemoryLogger<T>
    [ Fact ]
    public void Class_DerivesFromInMemoryLogger()
    {
        // Arrange

        // Act
        var logger = new InMemoryLogger<InMemoryLoggerTest>();

        // Assert
        logger.Should().BeAssignableTo<InMemoryLogger>();
    }
    #endregion

    #region LogInformation
    [ Fact ]
    public void LogInformation()
    {
        // Arrange
        var logger = new InMemoryLogger();

        // Act
        logger.LogInformation( "Some message." );

        // Assert
        var currentThreadId = Environment.CurrentManagedThreadId;
        logger.Messages.Count.Should().Be( 1 );
        var message = logger.Messages[ 0 ];

        message.LogLevel.Should().Be( LogLevel.Information );
        message.ElapsedMilliseconds.Should().BeLessThan( 10 );
        message.ThreadId.Should().Be( currentThreadId );
        message.Text.Should().Be( "Some message." );

        message.ToString()
               .Should()
               .MatchRegex( $"^Information \\[([ ]+[0-9]+ ms)\\]\\[{currentThreadId}\\] --> Some message.$" );
    }
    #endregion

    #region LogError
    [ Fact ]
    public void LogError()
    {
        // Arrange
        var logger = new InMemoryLogger();

        // Act
        logger.LogError( "Some message." );

        // Assert
        var currentThreadId = Environment.CurrentManagedThreadId;
        logger.Messages.Count.Should().Be( 1 );
        var message = logger.Messages[ 0 ];

        message.LogLevel.Should().Be( LogLevel.Error );
        message.ElapsedMilliseconds.Should().BeLessThan( 10 );
        message.ThreadId.Should().Be( currentThreadId );
        message.Text.Should().Be( "Some message." );

        message.ToString()
               .Should()
               .MatchRegex( $"^Error \\[([ ]+[0-9]+ ms)\\]\\[{currentThreadId}\\] --> Some message.$" );
    }
    #endregion

    #region LogException
    [ Fact ]
    public void LogException()
    {
        // Arrange
        var logger = new InMemoryLogger();

        // Act
        logger.LogError( new Exception( "Foo not found." ), "Some message." );

        // Assert
        var currentThreadId = Environment.CurrentManagedThreadId;
        logger.Messages.Count.Should().Be( 1 );
        var message = logger.Messages[ 0 ];

        message.LogLevel.Should().Be( LogLevel.Error );
        message.ElapsedMilliseconds.Should().BeLessThan( 10 );
        message.ThreadId.Should().Be( currentThreadId );
        message.Text.Should().Be( "Some message." + Environment.NewLine + "System.Exception: Foo not found." );

        message.ToString()
               .Should()
               .MatchRegex( $"^Error \\[([ ]+[0-9]+ ms)\\]\\[{currentThreadId}\\] --> Some message." );
    }
    #endregion

    #region IsEnabled
    [ Theory ]
    [ InlineData( LogLevel.Critical ) ]
    [ InlineData( LogLevel.Debug ) ]
    [ InlineData( LogLevel.Error ) ]
    [ InlineData( LogLevel.Information ) ]
    [ InlineData( LogLevel.None ) ]
    [ InlineData( LogLevel.Trace ) ]
    [ InlineData( LogLevel.Warning ) ]
    public void IsEnabled_ThrowsNotSupported( LogLevel logLevel )
    {
        // Arrange
        var logger = new InMemoryLogger();

        // Act
        var result = logger.IsEnabled( logLevel );

        // Act & Assert
        result.Should().BeTrue();
    }
    #endregion

    #region WriteTo
    [ Fact ]
    public void WriteTo_WithTestOutputHelper_WritesMessages()
    {
        // Arrange
        var logger = new InMemoryLogger();
        logger.LogInformation( "First message." );
        logger.LogWarning( "Second message." );
        var outputHelper = new CapturingTestOutputHelper();

        // Act
        logger.WriteTo( outputHelper );

        // Assert
        outputHelper.Lines.Count.Should().Be( 2 );
        outputHelper.Lines[ 0 ].Should().Contain( "First message." );
        outputHelper.Lines[ 1 ].Should().Contain( "Second message." );
    }

    [ Fact ]
    public void WriteTo_WithAction_InvokesActionForEachMessage()
    {
        // Arrange
        var logger = new InMemoryLogger();
        logger.LogInformation( "Action message." );
        var lines = new List<string>();

        // Act
        logger.WriteTo( message => lines.Add( message.ToString() ) );

        // Assert
        lines.Count.Should().Be( 1 );
        lines[ 0 ].Should().Contain( "Action message." );
    }
    #endregion

    #region BeginScope
    [ Fact ]
    public void BeginScope_ThrowsNotSupported()
    {
        // Arrange
        var logger = new InMemoryLogger();

        // Act
        var instance = logger.BeginScope( new Foo() );

        // Assert
        instance.Should().NotBeNull();
        instance.GetType().Name.Should().Be( "NullScope" );
    }
    #endregion

    [ UsedImplicitly ]
    // ReSharper disable once RedundantTypeDeclarationBody
    private class Foo { }
}