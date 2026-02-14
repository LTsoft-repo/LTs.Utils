using System.Globalization;
using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.DependencyInjection.Resources;

// ToDo: Add tests.

namespace LTs.DependencyInjections.Extensions;

// ReSharper disable once GrammarMistakeInComment
/// <summary>
///     Extension methods for use with the <see cref="AutofacServiceProvider" />.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    ///     Tries to cast the instance of <see cref="ILifetimeScope" /> from <see cref="AutofacServiceProvider" /> when
    ///     possible.
    /// </summary>
    /// <param name="serviceProvider">The instance of <see cref="IServiceProvider" />.</param>
    /// <exception cref="InvalidOperationException">
    ///     Throws an <see cref="InvalidOperationException" /> when instance of
    ///     <see cref="IServiceProvider" /> can't be assigned to <see cref="AutofacServiceProvider" />.
    /// </exception>
    /// <returns>Returns the instance of <see cref="ILifetimeScope" /> exposed by <see cref="AutofacServiceProvider" />.</returns>
    public static ILifetimeScope GetAutofacRoot( this IServiceProvider serviceProvider )
        => serviceProvider is not AutofacServiceProvider autofacServiceProvider
               ? throw new InvalidOperationException( string.Format( CultureInfo.CurrentCulture,
                                                                     ServiceProviderExtensionsResources.WrongProviderType,
                                                                     serviceProvider.GetType() ) )
               : autofacServiceProvider.LifetimeScope;
}