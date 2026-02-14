using Autofac;
using LTs.TestUtils.Loggers;
using LTs.Web.Abstractions;

namespace LTs.Web.test;

public class HttpHandlerRegistrationExtensionsTest : DisposableTest
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public HttpHandlerRegistrationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void AddHttpHandler_RegistersHttpClientAndHttpHandler()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        containerBuilder.AddTestLogger( TestOutput );

        // Act
        containerBuilder.AddHttpHandler();

        // Assert
        var container = containerBuilder.Build();

        var httpClient = container.Resolve<HttpClient>();
        httpClient.Should().NotBeNull();

        var httpHandler = container.Resolve<IHttpHandler>();
        httpHandler.Should().NotBeNull();

        httpHandler.Should().BeAssignableTo<IHttpHandler>();
        httpHandler.Should().BeOfType<HttpHandler>();
    }
}