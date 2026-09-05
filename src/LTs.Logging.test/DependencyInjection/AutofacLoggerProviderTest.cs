using Autofac;
using Autofac.Core.Registration;
using LTs.Logging.DependencyInjection;
using LTs.Logging.test.Infrastructure;
using Microsoft.Extensions.Logging;

namespace LTs.Logging.test.DependencyInjection;

public class AutofacLoggerProviderTest : BaseTest
{
    public AutofacLoggerProviderTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Ctor
    [ Fact ]
    public void Constructor_CreatesProvider()
    {
        // Arrange
        var container = BuildContainer();

        // Act
        var provider = new AutofacLoggerProvider( container );

        // Assert
        provider.Should().NotBeNull();
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_Successes()
    {
        // Arrange
        var container = BuildContainer();
        var provider = new AutofacLoggerProvider( container );

        // Act
        var act = () => provider.Dispose();

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region CreateLogger
    [ Fact ]
    public void CreateLogger_TypeValid_ResolvesLogger()
    {
        // Arrange
        var container = BuildContainer();
        var provider = new AutofacLoggerProvider( container );

        var categoryName = typeof( TestCategory ).AssemblyQualifiedName!;

        // Act
        var logger = provider.CreateLogger( categoryName );

        // Assert
        logger.Should().NotBeNull();
        logger.Should().BeAssignableTo<ILogger<TestCategory>>();
    }

    [ Fact ]
    public void CreateLogger_TypeNotFound_ResolvesLoggerObject()
    {
        // Arrange
        var container = BuildContainer();
        var provider = new AutofacLoggerProvider( container );

        const string invalidCategory = "This.Type.Does.Not.Exist";

        // Act
        var logger = provider.CreateLogger( invalidCategory );

        // Assert
        logger.Should().NotBeNull();
        logger.Should().BeAssignableTo<ILogger<object>>();
    }

    [ Fact ]
    public void CreateLogger_LoggerNotRegistered_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();
        var container = builder.Build();

        var provider = new AutofacLoggerProvider( container );
        var categoryName = typeof( TestCategory ).AssemblyQualifiedName!;

        // Act
        Action act = () => provider.CreateLogger( categoryName );

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>();
    }
    #endregion

    private static IContainer BuildContainer()
    {
        var builder = new ContainerBuilder();

        builder.RegisterGeneric( typeof( Logger<> ) )
               .As( typeof( ILogger<> ) )
               .SingleInstance();

        builder.RegisterType<LoggerFactory>()
               .As<ILoggerFactory>()
               .SingleInstance();

        return builder.Build();
    }
}
