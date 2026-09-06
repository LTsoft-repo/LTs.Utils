using Autofac;
using LTs.Logging.DependencyInjection;
using LTs.Logging.test.Infrastructure;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace LTs.Logging.test.DependencyInjection;

[ Collection( "Sequential" ) ]
public class RegistrationExtensionsTest : BaseTest
{
    public RegistrationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region AddSerilog
    [ Fact ]
    public void AddSerilog_AddsSuccessfully()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        var container = builder.AddSerilog( new() ).Build();

        // Assert
        var logger = container.Resolve<ILoggerProvider>();

        logger.Should().NotBeNull();
        logger.Should().BeAssignableTo<SerilogLoggerProvider>();
    }

    [ Fact ]
    public void AddSerilog_ConfigurationNull_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        var act = () => builder.AddSerilog( null! ).Build();

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage( "Value cannot be null. (Parameter 'configuration')" );
    }
    #endregion
}
