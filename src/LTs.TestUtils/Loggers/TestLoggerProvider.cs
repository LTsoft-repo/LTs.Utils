using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.Loggers;

/// <summary>
///     Logger provider for xUnit tests.
/// </summary>
public class TestLoggerProvider : ILoggerProvider, ILogger
{
    private ITestOutputHelper? currentTestOutput;
    private readonly List<string> logs = [ ];
    private readonly IMessageSink messageSink;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TestLoggerProvider" /> class.
    /// </summary>
    /// <param name="messageSink">Message sink for the logger.</param>
    public TestLoggerProvider( IMessageSink messageSink )
        => this.messageSink = messageSink;

    /// <summary>
    ///     Gets the logs list of Logs.
    /// </summary>
    public IEnumerable<string> CoreToolsLogs => [ .. logs ];

    // This needs to be created/disposed per-test, so we can associate logs
    // with the specific running test.
    /// <summary>
    ///     Set the test output helper for the current test.
    /// </summary>
    /// <param name="testOutput"></param>
    /// <returns></returns>
    [ UsedImplicitly ]
    public IDisposable UseTestLogger( ITestOutputHelper testOutput )
    {
        // reset these every test
        currentTestOutput = testOutput;

        return new DisposableOutput( this );
    }

    /// <summary>
    ///     Begin a logging scope (not implemented).
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="state"></param>
    /// <returns>
    ///     <c>null</c>
    /// </returns>
    public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
        => null;

    /// <inheritdoc />
    public ILogger CreateLogger( string categoryName ) => this;

    /// <inheritdoc />
    public void Dispose() { }

    /// <summary>
    ///     Check if the log level is enabled.
    /// </summary>
    /// <param name="logLevel">Log level to check.</param>
    /// <returns>Always <c>true</c>.</returns>
    public bool IsEnabled( LogLevel logLevel ) => true;

    /// <inheritdoc />
    public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter )
    {
        try
        {
            var formattedString = formatter( state, exception ?? new Exception( "Unexpected empty exception" ) );
            messageSink.OnMessage( new DiagnosticMessage( formattedString ) );

            logs.Add( formattedString );
            currentTestOutput?.WriteLine( formattedString );
        }
        catch
        {
            // ignored
        }
    }

    private class DisposableOutput : IDisposable
    {
        private readonly TestLoggerProvider xunitLogger;

        public DisposableOutput( TestLoggerProvider xunitLogger )
            => this.xunitLogger = xunitLogger;

        public void Dispose()
            => xunitLogger.currentTestOutput = null;
    }
}