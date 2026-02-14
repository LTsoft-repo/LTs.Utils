using JetBrains.Annotations;
using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Extensions;

public class LoadConfigurationExtensionsTest
{
    #region AddDefaultConfigurationForAssembly
    [ Fact ]
    public void AddDefaultConfigurationForAssembly_WithoutAdditionalConfiguration_AddsConfigurationsInCorrectOrder()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddDefaultConfigurationForAssembly<Foo>( null );

        // Assert
        var sources = builder.Sources;
        sources.Count.Should().Be( 4 );

        var sourceNames = sources.Select( s => s.GetType().Name )
                                 .ToArray();

        sourceNames.Should().ContainInOrder( "JsonConfigurationSource",                   // appsettings.json
                                             "JsonConfigurationSource",                   // appsettings.<Environment>.json
                                             "WrappedConfigurationSource",                // User Secrets (with EmptyString wrapper)
                                             "EnvironmentVariablesConfigurationSource" ); // Environment Variables           
    }

    [ Fact ]
    public void AddDefaultConfigurationForAssembly_WithAdditionalConfiguration_AddsConfigurationsInCorrectOrder()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddDefaultConfigurationForAssembly<Foo>( cb =>
                                                             cb.AddInMemoryCollection( new Dictionary<string, string?> { { "key", "value" } } ) );

        // Assert
        var sources = builder.Sources;
        sources.Count.Should().Be( 5 );

        var sourceNames = sources.Select( s => s.GetType().Name )
                                 .ToArray();

        sourceNames.Should().ContainInOrder( "JsonConfigurationSource",                 // appsettings.json
                                             "JsonConfigurationSource",                 // appsettings.<Environment>.json
                                             "WrappedConfigurationSource",              // User Secrets (with EmptyString wrapper)
                                             "EnvironmentVariablesConfigurationSource", // Environment Variables 
                                             "MemoryConfigurationSource" );             // Additional Configuration
    }
    #endregion

    [ UsedImplicitly ]
    // ReSharper disable once RedundantTypeDeclarationBody
    private class Foo { }
}