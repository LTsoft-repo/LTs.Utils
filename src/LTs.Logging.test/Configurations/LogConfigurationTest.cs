using LTs.Logging.Configurations;

namespace LTs.Logging.test.Configurations;

public class LogConfigurationTest : BaseTest
{
    public LogConfigurationTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void LogConfiguration_AssignsDefaultsSuccessfully()
    {
        // Arrange
        var expectedLogConfiguration = new LogConfiguration
        {
            Path = LogConfigurationDefaults.Path,
            MaxFileSizeInMegabytes = LogConfigurationDefaults.MaxFileSizeInMegabytes,
            DebugLogRetainedFileCount = LogConfigurationDefaults.DebugLogRetainedFileCount,
            ErrorLogRetainedFileCount = LogConfigurationDefaults.ErrorLogRetainedFileCount
        };

        // Act
        var logConfiguration = new LogConfiguration();

        // Assert
        logConfiguration.Should().NotBeNull();
        logConfiguration.Should().BeEquivalentTo( expectedLogConfiguration );
    }
}
