using Autofac;
using Autofac.Builder;
using JetBrains.Annotations;
using LTs.Stores.Abstractions;

namespace LTs.Stores.DependencyInjection;

/// <summary>
///     Registers JSON file store services with Autofac.
/// </summary>
public static class RegistrationExtensions
{
    /// <summary>
    ///     Registers <typeparamref name="TPathProvider" />, <see cref="JsonFileStore{TEntity}" />, and exposes
    ///     <see cref="JsonFileStore{TEntity}" /> as <see cref="IStore{TEntity}" />.
    /// </summary>
    /// <typeparam name="TEntity">The persisted entity type.</typeparam>
    /// <typeparam name="TPathProvider">The path provider implementation.</typeparam>
    /// <param name="builder">The Autofac container builder.</param>
    /// <returns>The container builder.</returns>
    [ UsedImplicitly ]
    public static IRegistrationBuilder<JsonFileStore<TEntity>, ConcreteReflectionActivatorData, SingleRegistrationStyle>
        AddJsonFileStore<TEntity, TPathProvider>( this ContainerBuilder builder )
        where TEntity : class
        where TPathProvider : class, IStorePathProvider<TEntity>
    {
        RegisterPathProvider<TEntity, TPathProvider>( builder );

        var registration = builder.RegisterType<JsonFileStore<TEntity>>()
                                  .As<IStore<TEntity>>()
                                  .SingleInstance();

        return registration;
    }

    [ UsedImplicitly ]
    private static IRegistrationBuilder<TPathProvider, ConcreteReflectionActivatorData, SingleRegistrationStyle>
        RegisterPathProvider<TEntity, TPathProvider>( ContainerBuilder builder )
        where TEntity : class
        where TPathProvider : class, IStorePathProvider<TEntity>
    {
        var registration = builder.RegisterType<TPathProvider>()
                                  .As<IStorePathProvider<TEntity>>()
                                  .SingleInstance();

        if( typeof( IListableStorePathProvider<TEntity> ).IsAssignableFrom( typeof( TPathProvider ) ) )
        {
            registration.As<IListableStorePathProvider<TEntity>>();
        }

        return registration;
    }
}