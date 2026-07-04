using Autofac;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.Loggers.DependencyInjection;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290

namespace LTs.TestUtils.test.Loggers.DependencyInjection;

public class RegistrationExtensionsTest : BaseTest
{
    public RegistrationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region AddTestLogger
    [ Fact ]
    public void AddTestLogger()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        builder.AddTestLogger( TestOutput );
        var container = builder.Build();

        // Assert
        var resolveAct = () => container.Resolve<ILogger<object>>();
        resolveAct.Should().NotThrow();

        var service = resolveAct.Invoke();
        service.Should().NotBeNull();
        service.Should().BeOfType<TestLogger<object>>();
    }
    #endregion
}