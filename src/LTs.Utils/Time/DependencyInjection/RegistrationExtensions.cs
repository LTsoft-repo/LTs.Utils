using Autofac;
using LTs.Utils.Time.Abstractions;

namespace LTs.Utils.Time.DependencyInjection;

/// <summary>
///     Registers Core time services.
/// </summary>
public static class RegistrationExtensions
{
    #region Services
    /// <summary>
    ///     Registers <see cref="IDateTimeOffsetProvider" /> with <see cref="DateTimeOffsetProvider" />.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    /// <returns>The container builder.</returns>
    public static ContainerBuilder AddDateTimeOffsetProvider( this ContainerBuilder builder )
    {
        builder.RegisterType<DateTimeOffsetProvider>()
               .As<IDateTimeOffsetProvider>()
               .SingleInstance();

        return builder;
    }
    #endregion
}