using Autofac;
using Autofac.Core.Registration;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Moq;
using IConfigurationProvider = LTs.Configurations.Abstractions.IConfigurationProvider;

namespace LTs.Configurations.test;

public class ConfigurationProviderTests
{
    #region Get
    [ Fact ]
    public void Get_WithCorrectParameters_ReturnsConfiguration()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        IConfigurationProvider provider = new ConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Act
        var result = provider.Get<IConfiguration>();

        // Assert
        result.Should().NotBeNull();
        result[ "Key" ].Should().Be( "Value" );
    }

    [ Fact ]
    public void Get_WithConfigurationClass_ReturnsConfiguration()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        var testConfiguration = new TestConfiguration
        {
            TestValue = "Test"
        };

        IConfigurationProvider provider = new ConfigurationProvider( mockConfiguration.Object,
                                                                     b => b.Register( _ => testConfiguration ).SingleInstance() );

        // Act
        var result = provider.Get<TestConfiguration>();

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo( new TestConfiguration
        {
            TestValue = "Test"
        } );
    }

    [ Fact ]
    public void Get_NonExistingConfiguration_Throws()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        IConfigurationProvider provider = new ConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Act
        var act = () => provider.Get<TestConfiguration>();

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>()
           .WithMessage( "The requested service 'LTs.Configurations.test.ConfigurationProviderTests+TestConfiguration' has not been registered.*" );
    }

    [ Fact ]
    public void Get_NullIConfiguration_Throws()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        IConfigurationProvider provider = new ConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Act
        var act = () => provider.Get<TestConfiguration>();

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>()
           .WithMessage( "The requested service 'LTs.Configurations.test.ConfigurationProviderTests+TestConfiguration' has not been registered.*" );
    }
    #endregion

    private class TestConfiguration
    {
        [ UsedImplicitly ]
        public string TestValue { get; init; } = string.Empty;
    }
}