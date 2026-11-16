using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.DependencyInjection;

public class AutofacServiceProviderFactoryTest : BaseTest
{
    public AutofacServiceProviderFactoryTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region CreateBuilder
    [ Fact ]
    public void CreateBuilder_WithServiceCollection_RegistersServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();
        var factory = new AutofacServiceProviderFactory();

        // Act
        var containerBuilder = factory.CreateBuilder( services );
        var serviceProvider = factory.CreateServiceProvider( containerBuilder );
        var resolved = serviceProvider.GetRequiredService<SomeClass>();

        // Assert
        resolved.Should().BeOfType<SomeClass>();
    }
    #endregion

    #region CreateServiceProvider
    [ Fact ]
    public void CreateServiceProvider_WithNullContainerBuilder_ThrowsArgumentNullException()
    {
        // Arrange
        var factory = new AutofacServiceProviderFactory();

        // Act
        var act = () => factory.CreateServiceProvider( null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName( "containerBuilder" );
    }
    #endregion

    #region Populate
    [ Fact ]
    public void Populate_AutofacDITest_Successes()
    {
        // Arrange
        var extraContainerBuilder = new ContainerBuilder();
        extraContainerBuilder.RegisterType<SomeConfigurationClass>();
        var extraContainer = extraContainerBuilder.Build();

        // Act
        var serviceProviderFactory = new AutofacServiceProviderFactory( b =>
            {
                foreach( var registration in extraContainer.ComponentRegistry.Registrations )
                {
                    b.RegisterComponent( registration );
                }
            } );

        var containerBuilder = serviceProviderFactory.CreateBuilder( new ServiceCollection() );
        containerBuilder.RegisterType<SomeServiceClass>();
        var serviceProvider = serviceProviderFactory.CreateServiceProvider( containerBuilder );

        // Assert
        serviceProvider.Should().NotBeNull();

        serviceProvider.GetRequiredService<SomeConfigurationClass>()
                       .Should().NotBeNull();

        serviceProvider.GetRequiredService<SomeServiceClass>()
                       .Should().NotBeNull();
    }
    #endregion
}