using LTs.DependencyInjections.Abstractions;

namespace LTs.DependencyInjections.Registrations;

// ToDo: Move to LTs.Utils solution.

/// <summary>
///     Base registration for a singleton service implementation.
/// </summary>
/// <typeparam name="TService">The service abstraction.</typeparam>
/// <typeparam name="TImplementation">The service implementation.</typeparam>
public abstract class Registration<TService, TImplementation> : IRegistration
    where TService : notnull
    where TImplementation : notnull, TService
{
    /// <inheritdoc />
    public virtual void Register( IRegistrationBuilder builder )
        => builder.RegisterSingleton<TService, TImplementation>();
}