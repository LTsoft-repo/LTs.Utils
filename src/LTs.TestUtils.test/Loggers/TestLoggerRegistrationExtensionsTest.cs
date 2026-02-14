using Autofac;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.test.Loggers;

public class TestLoggerRegistrationExtensionsTest : DisposableTest
{
    public TestLoggerRegistrationExtensionsTest( ITestOutputHelper testOutput )
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