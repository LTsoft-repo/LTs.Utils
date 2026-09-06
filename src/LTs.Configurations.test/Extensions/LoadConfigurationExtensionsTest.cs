using LTs.Configurations.Extensions;
using LTs.Configurations.test.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Extensions;

public class LoadConfigurationExtensionsTest : BaseTest
{
    public LoadConfigurationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region AddDefaultConfigurationForAssembly
    [ Fact ]
    public void AddDefaultConfigurationForAssembly_WithoutAdditionalConfiguration_AddsConfigurationsInCorrectOrder()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddDefaultConfigurationForAssembly<ReferenceType>( null );

        // Assert
        var sources = builder.Sources;
        var sourceCount = sources.Count;
        sourceCount.Should().Be( 4 );

        var sourceNames = sources.Select( s => s.GetType().Name )
                                 .ToArray();
        sourceNames.Should().ContainInOrder( "JsonConfigurationSource",
                                             "JsonConfigurationSource",
                                             "WrappedConfigurationSource",
                                             "EnvironmentVariablesConfigurationSource" );
    }

    [ Fact ]
    public void AddDefaultConfigurationForAssembly_WithAdditionalConfiguration_AddsConfigurationsInCorrectOrder()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddDefaultConfigurationForAssembly<ReferenceType>( cb =>
            cb.AddInMemoryCollection( new Dictionary<string, string?> { { "key", "value" } } ) );

        // Assert
        var sources = builder.Sources;
        var sourceCount = sources.Count;
        sourceCount.Should().Be( 5 );

        var sourceNames = sources.Select( s => s.GetType().Name )
                                 .ToArray();
        sourceNames.Should().ContainInOrder( "JsonConfigurationSource",
                                             "JsonConfigurationSource",
                                             "WrappedConfigurationSource",
                                             "EnvironmentVariablesConfigurationSource",
                                             "MemoryConfigurationSource" );
    }
    #endregion
}
