using Autofac;
using LTs.Configurations.Configurations;
using LTs.Configurations.Exceptions;
using LTs.Configurations.Extensions;
using LTs.Configurations.test.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Configurations;

public class TypedConfigurationLoaderTest : BaseTest
{
    public TypedConfigurationLoaderTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region LoadConfiguration
    [ Fact ]
    public void LoadConfiguration_BindsOptionalValuesAndDefaults()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddJsonString( """
                                            {
                                              "Sample": {
                                                "RequiredName": "primary",
                                                "OptionalCount": 10,
                                                "OptionalUri": "http://127.0.0.1:11434/",
                                                "OptionalTimeout": "00:10:00"
                                              }
                                            }
                                            """ )
                            .Build();

        // Act
        var result = configuration.LoadConfiguration<SampleConfiguration>( "Sample" );

        // Assert
        result.Should().BeEquivalentTo(
            new SampleConfiguration
            {
                RequiredName = "primary",
                OptionalDescription = "default-description", // Default value
                OptionalCount = 10,
                OptionalUri = new Uri( "http://127.0.0.1:11434/" ),
                OptionalTimeout = TimeSpan.FromMinutes( 10 )
            } );
    }

    [ Fact ]
    public void LoadConfiguration_WhenRequiredIsMissing_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddJsonString( """
                                            {
                                              "Sample": {
                                                "OptionalCount": 10
                                              }
                                            }
                                            """ )
                            .Build();

        // Act
        var act = () => configuration.LoadConfiguration<SampleConfiguration>( "Sample" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'Sample:RequiredName' not defined." );
    }

    [ Fact ]
    public void LoadConfiguration_WhenRequiredIsWhitespace_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddJsonString( """
                                            {
                                              "RequiredString": {
                                                "RequiredValue": "   "
                                              }
                                            }
                                            """ )
                            .Build();

        // Act
        var act = () => configuration.LoadConfiguration<RequiredStringConfiguration>( "RequiredString" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'RequiredString:RequiredValue' cannot be null or empty." );
    }

    [ Fact ]
    public void LoadConfiguration_WhenSectionIsMissing_ThrowsConfigurationException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var act = () => configuration.LoadConfiguration<SampleConfiguration>( "Missing" );

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration section 'Missing' not defined." );
    }
    #endregion

    #region AddConfiguration
    [ Fact ]
    public void AddConfiguration_ResolvesConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddJsonString( """
                                            {
                                              "Sample": {
                                                "RequiredName": "primary"
                                              }
                                            }
                                            """ )
                            .Build();

        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterInstance( configuration )
                        .As<IConfiguration>()
                        .SingleInstance();

        containerBuilder.AddConfiguration<SampleConfiguration>( "Sample" );

        using var container = containerBuilder.Build();

        // Act
        var result = container.Resolve<SampleConfiguration>();

        // Assert
        result.Should().BeEquivalentTo(
            new SampleConfiguration
            {
                RequiredName = "primary"
            } );
    }

    [ Fact ]
    public void AddConfiguration_AppliesPostLoadConfigurationAction()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddJsonString( """
                                            {
                                              "Sample": {
                                                "RequiredName": "primary",
                                                "OptionalDescription": "%TEMP%\\work"
                                              }
                                            }
                                            """ )
                            .Build();

        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterInstance( configuration )
                        .As<IConfiguration>()
                        .SingleInstance();

        containerBuilder.AddConfiguration<SampleConfiguration>(
            "Sample",
            loaded => loaded with
            {
                OptionalDescription = Environment.ExpandEnvironmentVariables( loaded.OptionalDescription )
            } );

        using var container = containerBuilder.Build();

        // Act
        var result = container.Resolve<SampleConfiguration>();

        // Assert
        var expectedDescription = Environment.ExpandEnvironmentVariables( "%TEMP%\\work" );

        result.Should().BeEquivalentTo( new SampleConfiguration
        {
            RequiredName = "primary",
            OptionalDescription = expectedDescription
        } );
    }
    #endregion
}