using Autofac;
using LTs.DependencyInjections.Extensions;
using LTs.DependencyInjections.test.Infrastructure;

namespace LTs.DependencyInjections.test.Extensions;

public class AutofacPopulateExtensionsTest
{
    #region Populate
    [ Fact ]
    public void Populate_WithTypeRegisteredInContainer_Successes()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterType<SomeClass>();
        var container = containerBuilder.Build();

        var builder = new ContainerBuilder();

        // Act
        var result = builder.Populate( container );
        var resultContainer = result.Build();

        // Assert
        resultContainer.IsRegistered<SomeClass>().Should().BeTrue();
    }

    [ Fact ]
    public void Populate_WithEmptyContainer_Successes()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();

        var builder = new ContainerBuilder();

        // Act
        var result = builder.Populate( container );
        var act = () => result.Build();

        // Assert
        act.Should().NotThrow();
    }
    #endregion
}