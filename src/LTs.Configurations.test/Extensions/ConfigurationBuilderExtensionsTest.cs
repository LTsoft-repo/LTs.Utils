using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Extensions;

public class ConfigurationBuilderExtensionsTest : BaseTest
{
    public ConfigurationBuilderExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void AddJsonString_AddsJsonConfiguration()
    {
        // Arrange
        var builder = new ConfigurationBuilder();
        var jsonString = """{ "Key": "Value" }""";

        // Act
        builder.AddJsonString( jsonString );
        var configuration = builder.Build();

        // Assert
        var value = configuration[ "Key" ];
        value.Should().Be( "Value" );
    }
}
