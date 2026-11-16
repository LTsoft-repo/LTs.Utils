using System.Reflection;
using LTs.DependencyInjections.Abstractions;

namespace LTs.DependencyInjections.Registrations;

// ToDo: Move to LTs.Utils solution.

/// <summary>
///     Discovers and applies <see cref="IRegistration" /> implementations from an assembly.
/// </summary>
public static class RegistrationsLoader
{
    /// <summary>
    ///     Applies all registrations discovered in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="builder">The registration builder.</param>
    public static void Apply( Assembly assembly, IRegistrationBuilder builder )
    {
        foreach( var registration in Discover( assembly ) )
        {
            registration.Register( builder );
        }
    }

    /// <summary>
    ///     Discovers all concrete <see cref="IRegistration" /> implementations in the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>The discovered registrations.</returns>
    public static IEnumerable<IRegistration> Discover( Assembly assembly )
        => assembly.GetTypes()
                   .Where( type => typeof( IRegistration ).IsAssignableFrom( type )
                                   && type is { IsAbstract: false, IsInterface: false } )
                   .Select( type => (IRegistration)Activator.CreateInstance( type )! );
}