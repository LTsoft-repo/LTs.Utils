// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.DependencyInjection;

/// <summary>
///     Autofac's implementation of the ASP.NET Core <see cref="IServiceScope" />.
/// </summary>
/// <seealso cref="IServiceScope" />
[ UsedImplicitly ]
internal class AutofacServiceScope : IServiceScope, IAsyncDisposable
{
    private bool disposed;
    private readonly Autofac.Extensions.DependencyInjection.AutofacServiceProvider serviceProvider;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacServiceScope" /> class.
    /// </summary>
    /// <param name="lifetimeScope">
    ///     The lifetime scope from which services should be resolved for this service scope.
    /// </param>
    public AutofacServiceScope( ILifetimeScope lifetimeScope )
        => serviceProvider = new Autofac.Extensions.DependencyInjection.AutofacServiceProvider( lifetimeScope );

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Gets an <see cref="IServiceProvider" /> corresponding to this service scope.
    /// </summary>
    /// <value>
    ///     An <see cref="IServiceProvider" /> that can be used to resolve dependencies from the scope.
    /// </value>
    public IServiceProvider ServiceProvider => serviceProvider;

    /// <summary>
    ///     Disposes of the lifetime scope and resolved disposable services.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    ///     Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
    ///     unmanaged resources.
    /// </param>
    protected virtual void Dispose( bool disposing )
    {
        if( !disposed )
        {
            disposed = true;

            if( disposing )
            {
                serviceProvider.Dispose();
            }
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if( !disposed )
        {
            disposed = true;
            await serviceProvider.DisposeAsync().ConfigureAwait( false );
        }
    }
}