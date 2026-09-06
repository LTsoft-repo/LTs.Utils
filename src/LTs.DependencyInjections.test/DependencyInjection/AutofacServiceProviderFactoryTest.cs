using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.DependencyInjection;

public class AutofacServiceProviderFactoryTest
{
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