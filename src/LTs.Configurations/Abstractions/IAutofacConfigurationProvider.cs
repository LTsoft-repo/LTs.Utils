using Autofac;

// ReSharper disable once CheckNamespace
namespace LTs.Configurations.Abstractions;

/// <summary>
///     Configuration provider implemented with Autofac.
/// </summary>
public interface IAutofacConfigurationProvider : IConfigurationProvider
{
    /// <summary>
    ///     Gets the Autofac container.
    /// </summary>
    /// <returns></returns>
    IContainer GetContainer();
}