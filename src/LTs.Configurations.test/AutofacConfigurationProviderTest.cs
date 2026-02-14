using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using JetBrains.Annotations;
using LTs.Configurations.Abstractions;
using LTs.Configurations.Extensions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LTs.Configurations.test;

public class AutofacConfigurationProviderTest
{
    #region Ctor
    [ Fact ]
    public void Ctor_WithCorrectParameters_Successes()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        // Act
        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Assert
        provider.Should().NotBeNull();
    }

    [ Fact ]
    public void Ctor_UsingConfigurationLoader_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddDefaultConfigurationForAssembly<SomeAssemblyClass>( configBuilder =>
                                                                                        configBuilder.AddInMemoryCollection(
                                                                                            new Dictionary<string, string?> { { "Key", "Value" } } )
                            ).Build();

        // Act
        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( configuration, _ => { } );

        // Assert
        provider.Should().NotBeNull();
        provider.Get<IConfiguration>()[ "Key" ].Should().Be( "Value" );
    }
    #endregion

    #region Get
    [ Fact ]
    public void Get_WithCorrectParameters_ReturnsConfiguration()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( mockConfiguration.Object, _ => { } );

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

        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( mockConfiguration.Object,
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

        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Act
        var act = () => provider.Get<TestConfiguration>();

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>()
           .WithMessage(
               "The requested service 'LTs.Configurations.test.AutofacConfigurationProviderTest+TestConfiguration' has not been registered.*" );
    }

    [ Fact ]
    public void Get_NullIConfiguration_Throws()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider( mockConfiguration.Object, _ => { } );

        // Act
        var act = () => provider.Get<TestConfiguration>();

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>()
           .WithMessage(
               "The requested service 'LTs.Configurations.test.AutofacConfigurationProviderTest+TestConfiguration' has not been registered.*" );
    }
    #endregion

    #region GetContainerBuilder
    [ Fact ]
    public void GetContainerBuilder_WithCorrectParameters_ReturnsContainerBuilder()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfigurationRoot>();
        mockConfiguration.Setup( x => x[ "Key" ] ).Returns( "Value" );

        var testConfiguration = new TestConfiguration
        {
            TestValue = "Test"
        };

        IAutofacConfigurationProvider provider = new AutofacConfigurationProvider(
            mockConfiguration.Object,
            b => b.Register( _ => testConfiguration ).SingleInstance() );

        // Act
        var result = provider.GetContainer();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IContainer>();
        result.Should().BeOfType<Container>();

        result.IsRegistered<IConfiguration>().Should().BeTrue();

        var configuration = result.Resolve<IConfiguration>();
        configuration[ "Key" ].Should().Be( "Value" );

        result.IsRegistered<TestConfiguration>().Should().BeTrue();
    }
    #endregion

    private class TestConfiguration
    {
        [ UsedImplicitly ]
        public string TestValue { get; init; } = string.Empty;
    }

    [ UsedImplicitly ]
    // ReSharper disable once RedundantTypeDeclarationBody
    private class SomeAssemblyClass { }
}