using Autofac;
using LTs.Configurations.Configurations;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Configurations;

public class AppConfigConfigurationLoaderTest : BaseTest
{
    public AppConfigConfigurationLoaderTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void LoadAppConfigConfiguration_WithConnectionString_ReturnsConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:AppConfig", "Endpoint=https://example.appconfig.io" }
                            } )
                            .Build();
        var expected = new AppConfigConfiguration
        {
            ConnectionString = "Endpoint=https://example.appconfig.io"
        };

        // Act
        var result = configuration.LoadAppConfigConfiguration();

        // Assert
        result.Should().BeEquivalentTo( expected );
    }

    [ Fact ]
    public void AddAppConfigConfigurationLoader_RegistersAppConfigConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "ConnectionStrings:AppConfig", "Endpoint=https://example.appconfig.io" }
                            } )
                            .Build();
        var expected = new AppConfigConfiguration
        {
            ConnectionString = "Endpoint=https://example.appconfig.io"
        };
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterInstance( configuration )
                        .As<IConfiguration>()
                        .SingleInstance();
        containerBuilder.AddAppConfigConfigurationLoader();

        // Act
        var container = containerBuilder.Build();
        var result = container.Resolve<AppConfigConfiguration>();

        // Assert
        result.Should().BeEquivalentTo( expected );
    }
}
