using LTs.Logging.Serilog.Sinks;
using LTs.Logging.test.Infrastructure;
using Moq;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;

namespace LTs.Logging.test.Serilog.Sinks;

public class PeriodicFlushSinkTest : BaseTest
{
    public PeriodicFlushSinkTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Constructor
    [ Fact ]
    public void Ctor_ValidSink_CreatesInstance()
    {
        // Arrange
        var sink = new TestFlushableSink();
        var flushInterval = TimeSpan.FromSeconds( 1 );

        // Act
        var result = new PeriodicFlushSink<TestFlushableSink>( sink, flushInterval );

        // Assert
        result.Should().NotBeNull();
    }

    [ Fact ]
    public void Ctor_NullSink_ThrowsArgumentNullException()
    {
        // Arrange
        TestFlushableSink sink = null!;
        var flushInterval = TimeSpan.FromSeconds( 1 );

        // Act
        Action act = () => new PeriodicFlushSink<TestFlushableSink>( sink, flushInterval );

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage( "Value cannot be null. (Parameter 'sink')" );
    }

    [ Theory ]
    [ InlineData( 0 ) ]
    [ InlineData( -1 ) ]
    public void Ctor_InvalidInterval_Throws( int interval )
    {
        // Arrange
        var sink = new TestFlushableSink();
        var flushInterval = TimeSpan.FromSeconds( interval );

        // Act
        Action act = () => new PeriodicFlushSink<TestFlushableSink>( sink, flushInterval );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage( "The flush interval must be greater than zero. (Parameter 'flushInterval')" );
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_Invoked_Successes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();
        sut.Emit( logEvent );

        // Act
        var act = () => sut.Dispose();

        // Assert
        act.Should().NotThrow();
        sink.Verify( x => x.Flush(), Times.Once );
    }

    [ Fact ]
    public void Dispose_InvokedTwice_FlushesOnceOnly()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        // Act
        var act = () =>
            {
                sut.Dispose();
                sut.Dispose();
            };

        // Assert
        act.Should().NotThrow();
        sink.Verify( x => x.Flush(), Times.Once );
    }
    #endregion

    #region Emit
    [ Fact ]
    public void Emit_ValidLogEvent_Successes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();

        // Act
        var act = () => sut.Emit( logEvent );

        // Assert
        act.Should().NotThrow();
        sink.Verify( x => x.Emit( logEvent ), Times.Once );
        sink.Verify( x => x.Flush(), Times.Never );
    }

    [ Fact ]
    public async Task Flush_WithEmmit_FlushesInASecond()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();

        // Act
        sut.Emit( logEvent );
        await Task.Delay( flushInterval.Add( TimeSpan.FromSeconds( 1 ) ) );

        // Assert
        sink.Verify( x => x.Emit( logEvent ), Times.Once );
        sink.Verify( x => x.Flush(), Times.Once );
    }

    [ Fact ]
    public void Emit_InvokedTwice_Successes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();

        // Act
        var act = () =>
            {
                sut.Emit( logEvent );
                sut.Emit( logEvent );
            };

        // Assert
        act.Should().NotThrow();
        sink.Verify( x => x.Emit( logEvent ), Times.Exactly( 2 ) );
    }

    [ Fact ]
    public void Emit_InvokedWithDisposedSink_DoesNotEmmit()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();

        // Act
        var act = () =>
            {
                sut.Dispose();
                sut.Emit( logEvent );
            };

        // Assert
        act.Should().NotThrow();
        sink.Verify( x => x.Emit( logEvent ), Times.Never );
    }

    [ Fact ]
    public void Emit_NullLogEvent_Throws()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new PeriodicFlushSink<TestFlushableSink>( sink.Object, flushInterval );

        // Act
        var act = () => sut.Emit( null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithMessage( "Value cannot be null. (Parameter 'logEvent')" );
    }
    #endregion

    #region Flush
    [ Fact ]
    public void Flush_WithEmit_Successes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new TestPeriodicFlushSink( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();
        sut.Emit( logEvent );

        // Act
        sut.FlushSink();

        // Assert
        sink.Verify( x => x.Flush(), Times.Once );
    }

    [ Fact ]
    public void Flush_WithDisposedSink_DoesNotFlushes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new TestPeriodicFlushSink( sink.Object, flushInterval );
        sut.Dispose();

        // Act
        sut.FlushSink();

        // Assert
        sink.Verify( x => x.Flush(), Times.Once );
    }

    [ Fact ]
    public void Flush_InvokedTwice_FlushesOnce()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new TestPeriodicFlushSink( sink.Object, flushInterval );

        var logEvent = CreateTestLogEvent();
        sut.Emit( logEvent );

        // Act
        sut.FlushSink();
        sut.FlushSink();

        // Assert
        sink.Verify( x => x.Flush(), Times.Once );
    }

    [ Fact ]
    public void Flush_WithNoEmmit_DoesNotFlushes()
    {
        // Arrange
        var sink = new Mock<TestFlushableSink>();

        var flushInterval = TimeSpan.FromSeconds( 1 );
        var sut = new TestPeriodicFlushSink( sink.Object, flushInterval );

        // Act
        sut.FlushSink();

        // Assert
        sink.Verify( x => x.Flush(), Times.Never );
    }
    #endregion

    private LogEvent CreateTestLogEvent()
    {
        var messageTemplate = new MessageTemplateParser().Parse( "Hello, {Name}! Today is {Day}." );

        return new( DateTimeOffset.Now, LogEventLevel.Information, null, messageTemplate, Array.Empty<LogEventProperty>() );
    }
}
