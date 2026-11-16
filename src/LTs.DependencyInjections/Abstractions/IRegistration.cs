namespace LTs.DependencyInjections.Abstractions;

/// <summary>
///     Registers services with a composition root without coupling to a specific container.
/// </summary>
public interface IRegistration
{
    /// <summary>
    ///     Registers services and dependencies.
    /// </summary>
    /// <param name="builder">The registration builder.</param>
    void Register( IRegistrationBuilder builder );
}