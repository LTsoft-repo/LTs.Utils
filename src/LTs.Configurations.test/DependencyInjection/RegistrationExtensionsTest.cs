using Autofac;
using LTs.Configurations.Abstractions;
using LTs.Configurations.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Moq;
using IConfigurationProvider = LTs.Configurations.Abstractions.IConfigurationProvider;

namespace LTs.Configurations.test.DependencyInjection;

public class RegistrationExtensionsTest
{
    #region AddConfigurationProvider (with register configuration action)
    [ Fact ]
    public void AddConfigurationProvider_ValidParameters_Successes()
    {
        // Arrange
        var mockRegisterConfigurationAction = new Mock<Action<ContainerBuilder>>();
        mockRegisterConfigurationAction.Setup( x => x( It.IsAny<ContainerBuilder>() ) );
        var registerConfigurationAction = mockRegisterConfigurationAction.Object;

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?> { { "Key", "Value" } } )
                            .Build();

        IConfigurationProvider configurationProvider = new TestConfigurationProvider( configuration, _ => { } );

        var builder = new ContainerBuilder();

        // Act
        builder.AddConfigurationProvider( configurationProvider, registerConfigurationAction );
        var container = builder.Build();

        // Assert
        container.IsRegistered<IConfigurationRoot>().Should().BeTrue();
        container.IsRegistered<IConfiguration>().Should().BeTrue();

        var configurationRootInstance = container.Resolve<IConfigurationRoot>();
        configurationRootInstance.Should().BeSameAs( configuration );

        var configurationInstance = container.Resolve<IConfiguration>();
        configurationInstance.Should().BeSameAs( configuration );

        mockRegisterConfigurationAction.Verify( x => x( It.IsAny<ContainerBuilder>() ), Times.Once );
    }

    [ Fact ]
    public void AddConfigurationProvider_WithoutRegistrationAction_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?> { { "Key", "Value" } } )
                            .Build();

        IConfigurationProvider configurationProvider = new TestConfigurationProvider( configuration, _ => { } );

        var builder = new ContainerBuilder();

        // Act
        var act = () => builder.AddConfigurationProvider( configurationProvider, null );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'registerConfigurationsAction')" );
    }
    #endregion

    #region AddConfigurationProvider (with IAutofacConfigurationProvider)
    [ Fact ]
    public void AddConfigurationProvider_WithAutofacConfigurationProvider_Successes()
    {
        var mockRegisterConfigurationAction = new Mock<Action<ContainerBuilder>>();
        mockRegisterConfigurationAction.Setup( x => x( It.IsAny<ContainerBuilder>() ) );
        var registerConfigurationAction = mockRegisterConfigurationAction.Object;

        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?> { { "Key", "Value" } } )
                            .Build();

        IAutofacConfigurationProvider configurationProvider = new AutofacConfigurationProvider( configuration, registerConfigurationAction );

        var builder = new ContainerBuilder();

        // Act
        builder.AddConfigurationProvider( configurationProvider );
        var container = builder.Build();

        // Assert
        container.IsRegistered<IConfiguration>().Should().BeTrue();

        var configurationRootInstance = container.Resolve<IConfigurationRoot>();
        configurationRootInstance.Should().BeSameAs( configuration );

        var configurationInstance = container.Resolve<IConfiguration>();
        configurationInstance.Should().BeSameAs( configuration );

        mockRegisterConfigurationAction.Verify( x => x( It.IsAny<ContainerBuilder>() ), Times.Once );
    }
    #endregion

    private class TestConfigurationProvider : IConfigurationProvider
    {
        private readonly IAutofacConfigurationProvider internalConfigurationProvider;

        // ReSharper disable once ConvertToPrimaryConstructor
        public TestConfigurationProvider( IConfigurationRoot configuration, Action<ContainerBuilder> registerConfigurationAction )
            => internalConfigurationProvider = new AutofacConfigurationProvider( configuration, registerConfigurationAction );

        public T Get<T>() where T : notnull => internalConfigurationProvider.Get<T>();
    }
}