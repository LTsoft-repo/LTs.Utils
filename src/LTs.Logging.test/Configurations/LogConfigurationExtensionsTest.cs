using Autofac;
using LTs.Logging.Configurations;
using LTs.Logging.DependencyInjection;
using LTs.Logging.test.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace LTs.Logging.test.Configurations;

[ Collection( "Sequential" ) ]
public class LogConfigurationExtensionsTest : BaseTest
{
    public LogConfigurationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region LoadLogConfiguration
    [ Fact ]
    public void LoadLogConfiguration_Success()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Logs:A:Path" ] = @"..\ThisPath",
                [ "Logs:A:MaxFileSizeInMegabytes" ] = "12",
                [ "Logs:A:DebugLogRetainedFileCount" ] = "34",
                [ "Logs:A:ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        var expectedLogConfiguration = new LogConfiguration
        {
            Path = @"..\ThisPath",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = 34,
            ErrorLogRetainedFileCount = 56
        };

        var section = configuration.GetSection( "Logs:A" );

        // Act
        var logConfiguration = section.LoadLogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }

    [ Fact ]
    public void LoadLogConfiguration_WithEnvironmentVariable_Success()
    {
        // Arrange
        Environment.SetEnvironmentVariable( "LOGS_FOLDER", @"ThisPath" );

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Logs:A:Path" ] = @"..\%LOGS_FOLDER%",
                [ "Logs:A:MaxFileSizeInMegabytes" ] = "12",
                [ "Logs:A:DebugLogRetainedFileCount" ] = "34",
                [ "Logs:A:ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        var expectedLogConfiguration = new LogConfiguration
        {
            Path = @"..\ThisPath",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = 34,
            ErrorLogRetainedFileCount = 56
        };

        var section = configuration.GetSection( "Logs:A" );

        // Act
        var logConfiguration = section.LoadLogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }

    [ Fact ]
    public void LoadLogConfiguration_WithEnvironmentVariableWithoutValue_Success()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Logs:A:Path" ] = @"..\%NON_ENV_VAR%",
                [ "Logs:A:MaxFileSizeInMegabytes" ] = "12",
                [ "Logs:A:DebugLogRetainedFileCount" ] = "34",
                [ "Logs:A:ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        var expectedLogConfiguration = new LogConfiguration
        {
            Path = @"..\%NON_ENV_VAR%",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = 34,
            ErrorLogRetainedFileCount = 56
        };

        var section = configuration.GetSection( "Logs:A" );

        // Act
        var logConfiguration = section.LoadLogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }

    [ Fact ]
    public void LoadLogConfiguration_NoConfiguration_GetsDefaults()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>() )
            .Build();

        var expectedLogConfiguration = new LogConfiguration
        {
            Path = LogConfigurationDefaults.Path,
            MaxFileSizeInMegabytes = LogConfigurationDefaults.MaxFileSizeInMegabytes,
            DebugLogRetainedFileCount = LogConfigurationDefaults.DebugLogRetainedFileCount,
            ErrorLogRetainedFileCount = LogConfigurationDefaults.ErrorLogRetainedFileCount
        };

        // Act
        var logConfiguration = configuration.LoadLogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }

    [ Fact ]
    public void LoadLogConfiguration_MissingConfiguration_GetsDefault()
    {
        // Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Path" ] = @"..\ThisPath",
                [ "MaxFileSizeInMegabytes" ] = "12",
                [ "ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        var expectedLogConfiguration = new LogConfiguration
        {
            Path = @"..\ThisPath",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = LogConfigurationDefaults.DebugLogRetainedFileCount,
            ErrorLogRetainedFileCount = 56
        };

        // Act
        var logConfiguration = configuration.LoadLogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }
    #endregion

    #region RegisterLogConfiguration
    [ Fact ]
    public void RegisterLogConfiguration_WithConfiguration_Successes()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Logs:A:Path" ] = @"..\ThisPath",
                [ "Logs:A:MaxFileSizeInMegabytes" ] = "12",
                [ "Logs:A:DebugLogRetainedFileCount" ] = "34",
                [ "Logs:A:ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        builder.Register<IConfiguration>( _ => config );

        // Act
        builder.RegisterLogConfiguration( "Logs:A" );

        // Assert
        var container = builder.Build();
        var logConfiguration = container.Resolve<LogConfiguration>();

        logConfiguration.Should().NotBeNull();

        logConfiguration.Should().BeEquivalentTo( new LogConfiguration
        {
            Path = @"..\ThisPath",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = 34,
            ErrorLogRetainedFileCount = 56
        } );
    }

    [ Fact ]
    public void RegisterLogConfiguration_NoConfiguration_Successes()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>() )
            .Build();

        builder.Register<IConfiguration>( _ => config );

        // Act
        builder.RegisterLogConfiguration( "Logs:A" );

        // Assert
        var container = builder.Build();
        var logConfiguration = container.Resolve<LogConfiguration>();

        logConfiguration.Should().NotBeNull();

        logConfiguration.Should().BeEquivalentTo( new LogConfiguration
        {
            Path = LogConfigurationDefaults.Path,
            MaxFileSizeInMegabytes = LogConfigurationDefaults.MaxFileSizeInMegabytes,
            DebugLogRetainedFileCount = LogConfigurationDefaults.DebugLogRetainedFileCount,
            ErrorLogRetainedFileCount = LogConfigurationDefaults.ErrorLogRetainedFileCount
        } );
    }

    [ Fact ]
    public void RegisterLogConfiguration_MissingConfiguration_Successes()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection( new Dictionary<string, string?>
            {
                [ "Logs:Path" ] = @"..\ThisPath",
                [ "Logs:MaxFileSizeInMegabytes" ] = "12",
                [ "Logs:ErrorLogRetainedFileCount" ] = "56"
            } )
            .Build();

        builder.Register<IConfiguration>( _ => config );

        // Act
        builder.RegisterLogConfiguration( "Logs" );

        // Assert
        var container = builder.Build();
        var logConfiguration = container.Resolve<LogConfiguration>();

        logConfiguration.Should().NotBeNull();

        logConfiguration.Should().BeEquivalentTo( new LogConfiguration
        {
            Path = @"..\ThisPath",
            MaxFileSizeInMegabytes = 12,
            DebugLogRetainedFileCount = LogConfigurationDefaults.DebugLogRetainedFileCount,
            ErrorLogRetainedFileCount = 56
        } );
    }
    #endregion
}
