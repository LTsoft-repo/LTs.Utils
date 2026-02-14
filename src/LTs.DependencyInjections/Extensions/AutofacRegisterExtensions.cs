using Autofac;
using Autofac.Builder;

namespace LTs.DependencyInjections.Extensions;

/// <summary>
///     Extensions to register types with Autofac.
/// </summary>
public static class AutofacRegisterExtensions
{
    /// <summary>
    ///     Register a generic type with Autofac simplifying the process.
    /// </summary>
    /// <typeparam name="TInterface">Interface to register.</typeparam>
    /// <typeparam name="TImplementation">Implementation for the interface to register.</typeparam>
    /// <param name="builder">The <see cref="ContainerBuilder" /> to register the type with.</param>
    /// <returns>
    ///     The <see cref="IRegistrationBuilder{Object, ReflectionActivatorData, DynamicRegistrationStyle}" /> for the
    ///     registered type.
    /// </returns>
    [ UsedImplicitly ]
    public static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle>
        RegisterGeneric<TInterface, TImplementation>( this ContainerBuilder builder )
        where TImplementation : notnull
        where TInterface : notnull
    {
        var typeInterface = typeof( TInterface );
        ValidateGenericType( typeInterface, nameof( TInterface ) );

        var typeImplementation = typeof( TImplementation );
        ValidateGenericType( typeImplementation, nameof( TImplementation ) );

        var genericTypeInterface = typeInterface.GetGenericTypeDefinition();
        var genericTypeImplementation = typeImplementation.GetGenericTypeDefinition();

        var regBuilder = builder
                         .RegisterGeneric( genericTypeImplementation )
                         .As( genericTypeInterface );

        return regBuilder;
    }

    /// <summary>
    ///     Validate if the type is a generic type.
    /// </summary>
    /// <param name="type">The type to validate.</param>
    /// <param name="paramName">The name of the parameter to be used in the exception message.</param>
    /// <exception cref="ArgumentException"></exception>
    private static void ValidateGenericType( Type type, string paramName )
    {
        if( !type.IsGenericType )
        {
            throw new ArgumentException( $"{paramName} is not a generic type." );
        }
    }
}