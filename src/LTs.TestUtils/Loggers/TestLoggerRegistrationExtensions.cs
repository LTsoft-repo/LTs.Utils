using Autofac;
using Microsoft.Extensions.Logging;

namespace LTs.TestUtils.Loggers;

/// <summary>
///     Extensions to register the <see cref="TestLogger{T}" />.
/// </summary>
public static class TestLoggerRegistrationExtensions
{
    /// <summary>
    ///     Registers the <see cref="TestLogger{T}" /> as <see cref="ILogger{T}" /> generic.
    /// </summary>
    /// <param name="builder">Autofac container builder.</param>
    /// <param name="testOutput">Test output helper.</param>
    public static void AddTestLogger( this ContainerBuilder builder, ITestOutputHelper testOutput )
    {
        builder.RegisterInstance( testOutput )
               .As<ITestOutputHelper>();

        builder.RegisterGeneric( typeof( TestLogger<> ) )
               .As( typeof( ILogger<> ) );
    }
}