using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.Loggers;

/// <summary>
///     Typed In-memory logger for tests.
/// </summary>
// ReSharper disable once RedundantTypeDeclarationBody
public class InMemoryLogger<T> : InMemoryLogger, ILogger<T> { }

/// <summary>
///     In-memory logger for tests.
/// </summary>
public class InMemoryLogger : ILogger
{
    /// <summary>
    ///     Gets the list of messages.
    /// </summary>
    public List<LoggerMessage> Messages => InternalMessages.ToList();

    /// <summary>
    ///     Internal message's queue.
    /// </summary>
    [ UsedImplicitly ]
    protected readonly ConcurrentQueue<LoggerMessage> InternalMessages = new();

    /// <summary>
    ///     Stop watch to measure time.
    /// </summary>
    [ UsedImplicitly ]
    protected readonly Stopwatch Stopwatch;

    /// <summary>
    ///     Initializes a new instance of the <see cref="InMemoryLogger" /> class.
    /// </summary>
    public InMemoryLogger()
    {
        Stopwatch = new Stopwatch();
        Stopwatch.Start();
    }

    /// <summary>
    ///     Log a message.
    /// </summary>
    /// <typeparam name="TState">Type of the state.</typeparam>
    /// <param name="logLevel">Log level.</param>
    /// <param name="eventId">Event ID.</param>
    /// <param name="state">State to log.</param>
    /// <param name="exception">Exception to log.</param>
    /// <param name="formatter">Formatter for the message.</param>
    public virtual void Log<TState>( LogLevel logLevel,
                                     EventId eventId,
                                     TState state,
                                     Exception? exception,
                                     Func<TState, Exception, string> formatter )
        => InternalMessages.Enqueue( new LoggerMessage
        {
            LogLevel = logLevel,
            ElapsedMilliseconds = Stopwatch.ElapsedMilliseconds,
            ThreadId = Thread.CurrentThread.ManagedThreadId,
            Text = $"{state}{( exception != null ? Environment.NewLine + exception : "" )}"
        } );

    /// <summary>
    ///     Check if the log level is enabled.
    /// </summary>
    /// <param name="logLevel">Log level to check.</param>
    /// <returns>Not supported.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public virtual bool IsEnabled( LogLevel logLevel ) => true;

    /// <summary>
    ///     Begin a logging scope (not implemented).
    /// </summary>
    /// <typeparam name="TState"> Type of the state.</typeparam>
    /// <param name="state">State to log.</param>
    /// <returns>A disposable object.</returns>
    public virtual IDisposable BeginScope<TState>( TState state ) where TState : notnull
        => NullScope.Instance;

    /// <summary>
    ///     Output the messages on the provided <see cref="ITestOutputHelper" />.
    /// </summary>
    /// <param name="testOutputHelper">Output helper to write to.</param>
    [ UsedImplicitly ]
    public virtual void WriteTo( ITestOutputHelper testOutputHelper )
        => WriteTo( m => testOutputHelper.WriteLine( m.ToString() ) );

    /// <summary>
    ///     Output the messages on the provided <see cref="Action{LoggerMessage}" />.
    /// </summary>
    /// <param name="actionWriteMessage">Action to write each message.</param>
    [ UsedImplicitly ]
    public virtual void WriteTo( Action<LoggerMessage> actionWriteMessage )
        => Messages.ForEach( actionWriteMessage );

    private class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}