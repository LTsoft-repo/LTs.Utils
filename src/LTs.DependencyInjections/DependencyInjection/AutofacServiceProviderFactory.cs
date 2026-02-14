// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac;
using Autofac.Builder;
using LTs.DependencyInjections.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.DependencyInjection;

/// <summary>
///     A factory for creating a <see cref="ContainerBuilder" /> and an <see cref="IServiceProvider" />.
/// </summary>
public class AutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    private readonly Action<ContainerBuilder> configurationAction;
    private readonly ContainerBuildOptions containerBuildOptions = ContainerBuildOptions.None;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacServiceProviderFactory" /> class.
    /// </summary>
    /// <param name="containerBuildOptions">The container options to use when building the container.</param>
    /// <param name="configurationAction">
    ///     Action on a <see cref="ContainerBuilder" /> that adds component registrations to the
    ///     container.
    /// </param>
    public AutofacServiceProviderFactory(
        ContainerBuildOptions containerBuildOptions,
        Action<ContainerBuilder>? configurationAction = null )
        : this( configurationAction ) =>
        this.containerBuildOptions = containerBuildOptions;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacServiceProviderFactory" /> class.
    /// </summary>
    /// <param name="configurationAction">
    ///     Action on a <see cref="ContainerBuilder" /> that adds component registrations to the
    ///     container.
    /// </param>
    public AutofacServiceProviderFactory( Action<ContainerBuilder>? configurationAction = null ) =>
        this.configurationAction = configurationAction ?? ( _ => { } );

    /// <summary>
    ///     Creates a container builder from an <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The collection of services.</param>
    /// <returns>A container builder that can be used to create an <see cref="IServiceProvider" />.</returns>
    public ContainerBuilder CreateBuilder( IServiceCollection services )
    {
        var builder = new ContainerBuilder();

        builder.Populate( services );

        configurationAction( builder );

        return builder;
    }

    /// <summary>
    ///     Creates an <see cref="IServiceProvider" /> from the container builder.
    /// </summary>
    /// <param name="containerBuilder">The container builder.</param>
    /// <returns>An <see cref="IServiceProvider" />.</returns>
    public IServiceProvider CreateServiceProvider( ContainerBuilder containerBuilder )
    {
        if( containerBuilder == null )
        {
            throw new ArgumentNullException( nameof( containerBuilder ) );
        }

        var container = containerBuilder.Build( containerBuildOptions );

        return new AutofacServiceProvider( container );
    }
}