using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.Loggers;

/// <summary>
///     Logger for tests.
/// </summary>
public class TestLogger : InMemoryLogger
{
    /// <summary>
    ///     The test output helper.
    /// </summary>
    [ UsedImplicitly ]
    protected readonly ITestOutputHelper TestOutput;

    /// <summary>
    ///     Creates a new instance of <see cref="TestLogger" />.
    /// </summary>
    /// <param name="testOutput"></param>
    // ReSharper disable once ConvertToPrimaryConstructor
    [ UsedImplicitly ]
    public TestLogger( ITestOutputHelper testOutput )
        => TestOutput = testOutput;

    /// <inheritdoc />
    public override void Log<TState>( LogLevel logLevel,
                                      EventId eventId,
                                      TState state,
                                      Exception? exception,
                                      Func<TState, Exception, string> formatter )
    {
        base.Log( logLevel, eventId, state, exception, formatter );

        var logLevelText = logLevel switch
        {
            LogLevel.Trace => "TRC",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WRN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "CRT",
            _ => "UNK"
        };

        var text = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff K} - [{logLevelText}] {GetText( state, exception )}";
        TestOutput.WriteLine( text );
        Debug.WriteLine( text );
    }

    /// <summary>
    ///     Gets the text to log.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual string GetText<TState>( TState state, Exception? exception )
        => $"{state}{( exception != null ? Environment.NewLine + exception : "" )}";
}

/// <summary>
///     Typed logger for tests.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TestLogger<T> : TestLogger, ILogger<T>
{
    /// <summary>
    ///     Creates a new instance of <see cref="TestLogger{T}" />.
    /// </summary>
    /// <param name="testOutput"></param>
    // ReSharper disable once ConvertToPrimaryConstructor
    public TestLogger( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    /// <inheritdoc />
    protected override string GetText<TState>( TState state, Exception? exception )
    {
        var text = base.GetText( state, exception );

        return @$"{text} {{""SourceContext"": ""{typeof( T ).FullName}""}}";
    }
}