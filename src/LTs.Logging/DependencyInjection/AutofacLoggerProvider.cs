using Autofac;
using Microsoft.Extensions.Logging;

// ReSharper disable ConvertToPrimaryConstructor

namespace LTs.Logging.DependencyInjection;

/// <summary>
///     Autofac implementation of <see cref="ILoggerProvider" />.
/// </summary>
public class AutofacLoggerProvider : ILoggerProvider
{
    private readonly IComponentContext context;

    /// <summary>
    ///     Creates a new <see cref="AutofacLoggerProvider" /> instance.
    /// </summary>
    /// <param name="context">The Autofac component context.</param>
    public AutofacLoggerProvider( IComponentContext context )
        => this.context = context;

    /// <inheritdoc />
    public void Dispose() { }

    /// <inheritdoc />
    public ILogger CreateLogger( string categoryName )
    {
        var genericLoggerType = typeof( ILogger<> ).MakeGenericType( Type.GetType( categoryName ) ?? typeof( object ) );

        return (ILogger)context.Resolve( genericLoggerType );
    }
}
