namespace LTs.DependencyInjections.Abstractions;

// ToDo: Move to LTs.Utils solution.

/// <summary>
///     Abstraction used by registrations to add services to a dependency injection container.
/// </summary>
public interface IRegistrationBuilder
{
    /// <summary>
    ///     Registers a singleton service implementation.
    /// </summary>
    /// <typeparam name="TService">The service abstraction.</typeparam>
    /// <typeparam name="TImplementation">The service implementation.</typeparam>
    void RegisterSingleton<TService, TImplementation>()
        where TService : notnull
        where TImplementation : notnull, TService;

    /// <summary>
    ///     Registers an agent-step tool-run descriptor.
    /// </summary>
    /// <param name="descriptor">The descriptor to register.</param>
    void RegisterDescriptor( IRegistrationDescriptor descriptor );
}