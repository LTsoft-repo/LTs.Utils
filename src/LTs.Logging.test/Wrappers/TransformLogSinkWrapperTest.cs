using LTs.Logging.Wrappers;
using LTs.Logging.test.Infrastructure;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace LTs.Logging.test.Wrappers;

[ Collection( "Sequential" ) ]
public class TransformLogSinkWrapperTest : BaseTest
{
    private readonly TextWriter oldWriter;

    protected readonly StringWriter StringWriter;

    public TransformLogSinkWrapperTest( ITestOutputHelper testOutput )
        : base( testOutput )
    {
        oldWriter = Console.Out;
        StringWriter = new();
        Console.SetOut( StringWriter );
    }

    public override void Dispose()
    {
        Console.SetOut( oldWriter );
        StringWriter.Dispose();
        base.Dispose();
    }

    #region TransformReplaceTextImpl
    [ Fact ]
    public void TransformReplaceTextImpl_ReplaceTextSuccessfully()
    {
        // Arrange

        // Act
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TransformLog( new[]
                {
                    new ReplaceTextLogTransformation( " information", "", LogEventLevel.Information )
                },
                writeTo => writeTo.Console() )
            .CreateLogger();

        // Assert
        var logger = Log.Logger;
        logger.Should().NotBeNull();
        logger.Information( "Some information log" );

        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\[\d{1,2}:\d{2}:\d{2} INF\] Some log" );
    }

    [ Fact ]
    public void TransformReplaceTextImpl_WithWrongLogEventLevel_DoesNotReplaceText()
    {
        // Arrange

        // Act
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TransformLog( new[]
                {
                    new ReplaceTextLogTransformation( " information", "", LogEventLevel.Error )
                },
                writeTo => writeTo.Console() )
            .CreateLogger();

        // Assert
        var logger = Log.Logger;
        logger.Should().NotBeNull();
        logger.Information( "Some information log" );

        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\[\d{1,2}:\d{2}:\d{2} INF\] Some information log" );
    }
    #endregion

    #region TransformLog
    [ Fact ]
    public void TransformLog_NoChanges_Successes()
    {
        // Arrange
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TransformLog( new[]
                {
                    new TestLogTransformation
                    {
                        ShouldTransform = _ => true,
                        Transform = logEvent => logEvent
                    }
                },
                writeTo => writeTo.Console() )
            .CreateLogger();

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        // Act
        logger.Information( "Some information log" );

        // Assert
        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\[\d{1,2}:\d{2}:\d{2} INF\] Some information log" );
    }

    [ Fact ]
    public void TransformLog_WithFunctions_Successes()
    {
        // Arrange
        bool Condition( LogEvent logEvent )
        {
            return logEvent.Level == LogEventLevel.Information &&
                   logEvent.MessageTemplate.Text.Contains( " information" );
        }

        LogEvent Transformation( LogEvent logEvent )
        {
            var newTokens = logEvent.MessageTemplate.Tokens.Select( t =>
                    {
                        if( t is TextToken token && token.Text.Contains( " information" ) )
                        {
                            return new TextToken( token.Text.Replace( " information", "" ) );
                        }

                        return t;
                    } )
                .ToList();

            return new( logEvent.Timestamp,
                logEvent.Level,
                logEvent.Exception,
                new( newTokens ),
                logEvent.Properties.Select( kvp => new LogEventProperty( kvp.Key, kvp.Value ) ) );
        }

        // Act
        Log.Logger = new LoggerConfiguration()
            .WriteTo.TransformLog( new[]
                {
                    new TestLogTransformation
                    {
                        ShouldTransform = Condition,
                        Transform = Transformation
                    }
                },
                writeTo => writeTo.Console() )
            .CreateLogger();

        // Assert
        var logger = Log.Logger;
        logger.Should().NotBeNull();
        logger.Information( "Some information log" );

        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\[\d{1,2}:\d{2}:\d{2} INF\] Some log" );
    }
    #endregion
}
