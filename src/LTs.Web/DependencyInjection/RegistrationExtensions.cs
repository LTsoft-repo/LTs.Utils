using Autofac;
using LTs.Web.Abstractions;

namespace LTs.Web.DependencyInjection;

/// <summary>
///     Extension methods for registering the HTTP handler services and dependencies.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    ///     Register the HTTP handler services and dependencies.
    /// </summary>
    /// <param name="builder">The container builder to register the services with.</param>
    /// <returns>The container builder with the services registered.</returns>
    [ UsedImplicitly ]
    public static ContainerBuilder AddHttpHandler( this ContainerBuilder builder )
    {
        builder.RegisterType<HttpClient>();

        builder.RegisterType<HttpHandler>()
               .As<IHttpHandler>();

        return builder;
    }
}