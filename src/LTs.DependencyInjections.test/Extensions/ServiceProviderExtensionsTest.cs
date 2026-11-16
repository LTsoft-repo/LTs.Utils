using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Extensions;

public class ServiceProviderExtensionsTest : BaseTest
{
    public ServiceProviderExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetAutofacRoot
    [ Fact ]
    public void GetAutofacRoot_WithAutofacServiceProvider_ReturnsRootLifetimeScope()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();
        var serviceProvider = new AutofacServiceProvider( container );

        // Act
        var root = serviceProvider.GetAutofacRoot();

        // Assert
        root.Should().BeSameAs( container );
    }

    [ Fact ]
    public void GetAutofacRoot_WithOtherServiceProvider_ThrowsInvalidOperationException()
    {
        // Arrange
        IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();

        // Act
        var act = () => serviceProvider.GetAutofacRoot();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage( "Unable to retrieve Autofac root lifetime scope from service provider of type*" );
    }
    #endregion
}