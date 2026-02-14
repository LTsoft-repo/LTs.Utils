// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using KeyedService = Autofac.Core.KeyedService;

namespace LTs.DependencyInjections.DependencyInjection;

/// <summary>
///     Autofac's implementation of the ASP.NET Core <see cref="IServiceProvider" />.
/// </summary>
/// <seealso cref="IServiceProvider" />
/// <seealso cref="ISupportRequiredService" />
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class AutofacServiceProvider :
    //IServiceProvider, ISupportRequiredService, IKeyedServiceProvider, IServiceProviderIsService,
    ISupportRequiredService, IKeyedServiceProvider,
    IServiceProviderIsKeyedService, IDisposable, IAsyncDisposable
{
    private bool disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacServiceProvider" /> class.
    /// </summary>
    /// <param name="lifetimeScope">
    ///     The lifetime scope from which services will be resolved.
    /// </param>
    public AutofacServiceProvider( ILifetimeScope lifetimeScope )
        => LifetimeScope = lifetimeScope;

    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType">
    ///     An object that specifies the type of service object to get.
    /// </param>
    /// <param name="serviceKey">
    ///     An object that specifies the key of service object to get.
    /// </param>
    /// <returns>
    ///     A service object of type <paramref name="serviceType" />; or <see langword="null" />
    ///     if there is no service object of type <paramref name="serviceType" />.
    /// </returns>
    public object? GetKeyedService( Type serviceType, object? serviceKey )
    {
        if( serviceKey is null )
        {
            // A null key equates to "not keyed."
            return LifetimeScope.ResolveOptional( serviceType );
        }

        try
        {
            return LifetimeScope.ResolveOptionalService( new KeyedService( serviceKey, serviceType ) );
        }
        catch( DependencyResolutionException ex ) when( ex.InnerException is KeyTypeConversionException conversionException )
        {
            // If the issue was with converting the specified key type to
            // match a [ServiceKey] parameter type, the M.E.DI contract is
            // that it must be an InvalidOperationException.
            throw new InvalidOperationException( conversionException.Message, conversionException );
        }
    }

    /// <summary>
    ///     Gets service of type <paramref name="serviceType" /> from the
    ///     <see cref="AutofacServiceProvider" /> and requires it be present.
    /// </summary>
    /// <param name="serviceType">
    ///     An object that specifies the type of service object to get.
    /// </param>
    /// <param name="serviceKey">
    ///     An object that specifies the key of service object to get.
    /// </param>
    /// <returns>
    ///     A service object of type <paramref name="serviceType" />.
    /// </returns>
    /// <exception cref="ComponentNotRegisteredException">
    ///     Thrown if the <paramref name="serviceType" /> isn't registered with the container.
    /// </exception>
    /// <exception cref="DependencyResolutionException">
    ///     Thrown if the object can't be resolved from the container.
    /// </exception>
    public object GetRequiredKeyedService( Type serviceType, object? serviceKey )
    {
        if( serviceKey is null )
        {
            // A null key equates to "not keyed."
            return LifetimeScope.Resolve( serviceType );
        }

        try
        {
            return LifetimeScope.ResolveKeyed( serviceKey, serviceType );
        }
        catch( DependencyResolutionException ex ) when( ex.InnerException is KeyTypeConversionException conversionException )
        {
            // If the issue was with converting the specified key type to
            // match a [ServiceKey] parameter type, the M.E.DI contract is
            // that it must be an InvalidOperationException.
            throw new InvalidOperationException( conversionException.Message, conversionException );
        }
    }

    /// <summary>
    ///     Gets service of type <paramref name="serviceType" /> from the
    ///     <see cref="AutofacServiceProvider" /> and requires it be present.
    /// </summary>
    /// <param name="serviceType">
    ///     An object that specifies the type of service object to get.
    /// </param>
    /// <returns>
    ///     A service object of type <paramref name="serviceType" />.
    /// </returns>
    /// <exception cref="ComponentNotRegisteredException">
    ///     Thrown if the <paramref name="serviceType" /> isn't registered with the container.
    /// </exception>
    /// <exception cref="DependencyResolutionException">
    ///     Thrown if the object can't be resolved from the container.
    /// </exception>
    public object GetRequiredService( Type serviceType )
        => LifetimeScope.Resolve( serviceType );

    /// <inheritdoc />
    public bool IsKeyedService( Type serviceType, object? serviceKey )
        // Null service key means non-keyed.
        => serviceKey == null
               ? IsService( serviceType )
               : LifetimeScope.ComponentRegistry.IsRegistered( new KeyedService( serviceKey, serviceType ) );

    /// <inheritdoc />
    public bool IsService( Type serviceType ) => LifetimeScope.ComponentRegistry.IsRegistered( new TypedService( serviceType ) );

    /// <summary>
    ///     Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType">
    ///     An object that specifies the type of service object to get.
    /// </param>
    /// <returns>
    ///     A service object of type <paramref name="serviceType" />; or <see langword="null" />
    ///     if there is no service object of type <paramref name="serviceType" />.
    /// </returns>
    public object? GetService( Type serviceType )
        => LifetimeScope.ResolveOptional( serviceType );

    /// <summary>
    ///     Gets the underlying instance of <see cref="ILifetimeScope" />.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public ILifetimeScope LifetimeScope { get; }

    /// <summary>
    ///     Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <see langword="true" /> to release both managed and unmanaged resources;
    ///     <see langword="false" /> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose( bool disposing )
    {
        if( !disposed )
        {
            disposed = true;

            if( disposing )
            {
                LifetimeScope.Dispose();
            }
        }
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    ///     Performs a dispose operation asynchronously.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if( !disposed )
        {
            disposed = true;
            await LifetimeScope.DisposeAsync().ConfigureAwait( false );
            GC.SuppressFinalize( this );
        }
    }
}