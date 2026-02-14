// Copyright (c) Autofac Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.DependencyInjection;

/// <summary>
///     Autofac's implementation of the ASP.NET Core <see cref="IServiceScopeFactory" />.
/// </summary>
/// <seealso cref="IServiceScopeFactory" />
internal class AutofacServiceScopeFactory : IServiceScopeFactory
{
    private readonly ILifetimeScope lifetimeScope;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AutofacServiceScopeFactory" /> class.
    /// </summary>
    /// <param name="lifetimeScope">The lifetime scope.</param>
    public AutofacServiceScopeFactory( ILifetimeScope lifetimeScope )
        => this.lifetimeScope = lifetimeScope;

    // ReSharper disable GrammarMistakeInComment
    /// <summary>
    ///     Creates an <see cref="IServiceScope" /> which contains an
    ///     <see cref="System.IServiceProvider" /> used to resolve dependencies within
    ///     the scope.
    /// </summary>
    /// <returns>
    ///     An <see cref="IServiceScope" /> controlling the lifetime of the scope. Once
    ///     this is disposed, any scoped services that have been resolved
    ///     from the <see cref="IServiceScope.ServiceProvider" />
    ///     will also be disposed.
    /// </returns>
    // ReSharper restore GrammarMistakeInComment
    public IServiceScope CreateScope()
        => new AutofacServiceScope( lifetimeScope.BeginLifetimeScope() );
}