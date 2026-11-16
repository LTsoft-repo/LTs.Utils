namespace LTs.DependencyInjections.Abstractions;

/// <summary>
///     Represents a descriptor for a service registration, including the service type, implementation type, name, and an
///     optional tag.
/// </summary>
public interface IRegistrationDescriptor
{
    /// <summary>
    ///     Gets the type of the service abstraction.
    /// </summary>
    Type ServiceType { get; }

    /// <summary>
    ///     Gets the type of the service implementation.
    /// </summary>
    Type ImplementationType { get; }

    /// <summary>
    ///     The tag associated with the service registration, if any.
    /// </summary>
    string? Tag { get; }
}