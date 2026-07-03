using Microsoft.Extensions.Configuration;
using ConfigurationManager = LTs.TestUtils.Configurations.ConfigurationManager;

namespace LTs.TestUtils.test.Configurations;

public class ConfigurationManagerTest
{
    [ Fact ]
    public void ConfigurationManager_Constructor_SetsConfiguration()
    {
        // Arrange
        var configurationManager = new ConfigurationManager( typeof( ConfigurationManagerTest ) );

        configurationManager.Configuration!.GetSection( "Section1" ).Exists().Should()
                            .BeTrue( "you must define User Secrets for the test. Check SecretsTemplate.json" );

        // Act
        var configuration = configurationManager.Configuration;

        // Assert
        configuration.Should().NotBeNull();

        // User Secrets
        configuration!.GetSection( "Section1" ).Should().NotBeNull();
        configuration.GetSection( "Section1" ).GetValue<string>( "Key1" ).Should().Be( "Value1" );

        // appsettings.json
        configuration.GetSection( "appSettings" ).Should().NotBeNull();
        configuration.GetSection( "appSettings" ).GetValue<string>( "TestString" ).Should().Be( "TestValue" );

        // Combination
        configuration.GetSection( "Section2" ).GetValue<int>( "Property2" ).Should().Be( 2 );
    }
}
