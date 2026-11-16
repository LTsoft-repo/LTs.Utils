using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Extensions;

public class ConfigurationBuilderEmptyStringExtensionsTest : BaseTest
{
    public ConfigurationBuilderEmptyStringExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ParseEmptyString
    [ Fact ]
    public void ParseEmptyString_ReplacesEmptyStringTagWithEmptyValue()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.ParseEmptyString( configurationBuilder =>
            configurationBuilder.AddInMemoryCollection( new Dictionary<string, string?>
            {
                { "Plain", "value" },
                { "EmptyTag", "<EmptyString>" },
                { "Embedded", "prefix<EmptyString>suffix" }
            } ) );
        var configuration = builder.Build();

        var plain = configuration[ "Plain" ];
        var emptyTag = configuration[ "EmptyTag" ];
        var embedded = configuration[ "Embedded" ];
        var missing = configuration[ "Missing" ];

        // Assert
        plain.Should().Be( "value" );
        emptyTag.Should().Be( string.Empty );
        embedded.Should().Be( "prefixsuffix" );
        missing.Should().BeNull();
    }

    [ Fact ]
    public void ParseEmptyString_BuildsWrappedConfiguration()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.ParseEmptyString( configurationBuilder =>
            configurationBuilder.AddInMemoryCollection( new Dictionary<string, string?> { { "Key", "value" } } ) );
        var configuration = builder.Build();

        // Assert
        var value = configuration[ "Key" ];
        value.Should().Be( "value" );

        builder.Sources.Should().ContainSingle( source => source.GetType().Name == "WrappedConfigurationSource" );
    }
    #endregion
}