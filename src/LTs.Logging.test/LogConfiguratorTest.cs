using System.Diagnostics;
using LTs.Logging;
using LTs.Logging.Configurations;
using LTs.Logging.Wrappers;
using LTs.Logging.test.Infrastructure;
using Serilog;
using Serilog.Events;

namespace LTs.Logging.test;

[ Collection( "Sequential" ) ]
public class LogConfiguratorTest : BaseTest
{
    protected static readonly Random RandomGenerator = new();

    protected readonly StringWriter StringWriter;

    protected readonly DateTime TimeNow;
    protected readonly string LogsFolderPath;
    protected readonly DirectoryInfo LogsFolder;

    private readonly TextWriter oldWriter;

    public LogConfiguratorTest( ITestOutputHelper testOutput )
        : base( testOutput )
    {
        oldWriter = Console.Out;
        StringWriter = new();
        Console.SetOut( StringWriter );

        TimeNow = DateTime.Now;
        LogsFolderPath = Path.Combine( Path.GetTempPath(), "LTs.Logging.test", Guid.NewGuid().ToString( "N" ) );
        Directory.CreateDirectory( LogsFolderPath );
        LogsFolder = new( LogsFolderPath );
    }

    public override void Dispose()
    {
        Console.SetOut( oldWriter );
        StringWriter.Dispose();
        CleanLogs( LogsFolder );

        if( LogsFolder.Exists )
        {
            LogsFolder.Delete( true );
        }

        base.Dispose();
    }

    private void CleanLogs( DirectoryInfo directory )
    {
        if( !directory.Exists )
        {
            return;
        }

        var logFiles = directory.GetFiles( "*.log" );

        if( logFiles.Length > 0 )
        {
            foreach( var file in logFiles )
            {
                file.Delete();
            }
        }
    }

    #region Configure
    [ Fact ]
    public void Configure_DebugFile_GetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath } );
        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some information log" );
        Log.CloseAndFlush();

        // Assert
        var logFile = LogsFolder.GetFiles( $"debug-{TimeNow:yyyyMMdd}*.log" ).LastOrDefault();
        logFile.Should().NotBeNull();

        var log = File.ReadAllLines( logFile!.FullName ).FirstOrDefault();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some information log \{\}" );

        CleanLogs( LogsFolder );
    }

    [ Fact ]
    public void Configure_ErrorFile_GetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath } );
        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Error( "This is an error log" );
        Log.CloseAndFlush();

        // Assert
        var logFile = LogsFolder.GetFiles( $"error-{TimeNow:yyyyMMdd}*.log" ).LastOrDefault();
        logFile.Should().NotBeNull();

        var log = File.ReadAllLines( logFile!.FullName ).FirstOrDefault();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[ERR\] This is an error log \{\}" );

        CleanLogs( LogsFolder );
    }

    [ Fact ]
    public void Configure_Console_GetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath } );
        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some console information log" );
        Log.CloseAndFlush();

        // Assert
        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some console information log \{\}" );
    }

    [ Fact ]
    public void Configure_Debug_GetsLogSuccessfully()
    {
        // Arrange
        using var stringWriter = new StringWriter();
        var myWriter = new TextWriterTraceListener( stringWriter );
        Trace.Listeners.Add( myWriter );

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath } );
        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some debug information log" );
        Log.CloseAndFlush();

        // Assert
        var log = stringWriter.ToString();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some debug information log \{\}" );
    }

    [ Fact ]
    public void Configure_AdditionalConfiguration_GetsFileLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath },
            lc => lc.WriteTo.File( Path.Combine( LogsFolderPath, "test.log" ) ) );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some information log" );
        Log.CloseAndFlush();

        // Assert
        var logFile = LogsFolder.GetFiles( "test.log" ).LastOrDefault();
        logFile.Should().NotBeNull();

        var log = File.ReadAllLines( logFile!.FullName ).FirstOrDefault();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some information log" );

        CleanLogs( LogsFolder );
    }

    [ Fact ]
    public void Configure_UsingFilter_RemovesLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.Configure( new() { Path = LogsFolderPath },
            lc =>
                {
                    lc.Filter.ByExcluding( le =>
                        {
                            var isFiltered = le.MessageTemplate.Text.Contains( "information" );

                            return isFiltered;
                        } );
                } );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some information log" );
        Log.CloseAndFlush();

        // Assert
        var log = StringWriter.ToString();
        log.Should().Be( "" );
    }
    #endregion

    #region ConfigureWithTransform
    [ Fact ]
    public void ConfigureWithTransform_ReplaceText_DebugFileGetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.ConfigureWithTransform(
            new() { Path = LogsFolderPath },
            new[] { new ReplaceTextLogTransformation( " information", "", LogEventLevel.Information ) },
            _ => { } );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some information log" );
        Log.CloseAndFlush();

        // Assert
        var logFile = LogsFolder.GetFiles( $"debug-{TimeNow:yyyyMMdd}*.log" ).LastOrDefault();
        logFile.Should().NotBeNull();

        var log = File.ReadAllLines( logFile!.FullName ).FirstOrDefault();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some log \{\}" );

        CleanLogs( LogsFolder );
    }

    [ Fact ]
    public void ConfigureWithTransform_ReplaceText_ErrorFileGetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.ConfigureWithTransform(
            new() { Path = LogsFolderPath },
            new[] { new ReplaceTextLogTransformation( "This is an error log", "Error log", LogEventLevel.Error ) },
            _ => { } );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Error( "This is an error log" );
        Log.CloseAndFlush();

        // Assert
        var logFile = LogsFolder.GetFiles( $"error-{TimeNow:yyyyMMdd}*.log" ).LastOrDefault();
        logFile.Should().NotBeNull();

        var log = File.ReadAllLines( logFile!.FullName ).FirstOrDefault();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[ERR\] Error log \{\}" );

        CleanLogs( LogsFolder );
    }

    [ Fact ]
    public void ConfigureWithTransform_ReplaceText_ConsoleGetsLogSuccessfully()
    {
        // Arrange

        // Act
        LogConfigurator.ConfigureWithTransform(
            new() { Path = LogsFolderPath },
            new[] { new ReplaceTextLogTransformation( " information", "", LogEventLevel.Information ) },
            _ => { } );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some console information log" );
        Log.CloseAndFlush();

        // Assert
        var log = StringWriter.ToString();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some console log \{\}" );
    }

    [ Fact ]
    public void ConfigureWithTransform_ReplaceText_DebugGetsLogSuccessfully()
    {
        // Arrange
        using var stringWriter = new StringWriter();
        var myWriter = new TextWriterTraceListener( stringWriter );
        Trace.Listeners.Add( myWriter );

        // Act
        LogConfigurator.ConfigureWithTransform(
            new() { Path = LogsFolderPath },
            new[] { new ReplaceTextLogTransformation( " information", "", LogEventLevel.Information ) },
            _ => { } );

        var logger = Log.Logger;
        logger.Should().NotBeNull();

        logger.Information( "Some debug information log" );
        Log.CloseAndFlush();

        // Assert
        var log = stringWriter.ToString();
        log.Should().MatchRegex( @"\d{4}-\d{2}-\d{2} \d{1,2}:\d{2}:\d{2}.\d{3} [+-]\d{1,2}:\d{2} \[INF\] Some debug log \{\}" );
    }
    #endregion
}
