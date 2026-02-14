using Autofac;

namespace LTs.Utils.Applications.Abstractions;

/// <summary>
///     Typed interface for the application logic.
/// </summary>
/// <typeparam name="T">The type the application logic is for.</typeparam>
public interface IAppLogic<T> : IAppLogic
    where T : class
// ReSharper disable once RedundantTypeDeclarationBody
{ }

/// <summary>
///     Interface for the application logic.
/// </summary>
public interface IAppLogic : IDisposable
{
    /// <summary>
    ///     Runs the application logic.
    /// </summary>
    /// <param name="appScope">The application service scope.</param>
    /// <returns></returns>
    Task RunAsync( ILifetimeScope appScope );
}