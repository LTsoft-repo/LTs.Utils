using Autofac;
using Autofac.Core.Registration;
using LTs.Stores.Abstractions;
using LTs.Stores.DependencyInjection;
using LTs.Stores.test.Infrastructure;

namespace LTs.Stores.test.DependencyInjection;

public class RegistrationExtensionsTest : BaseTest
{
    public RegistrationExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region AddJsonFileStore
    [ Fact ]
    public void AddJsonFileStore_RegistersStoreAndListablePathProvider()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        builder.AddJsonFileStore<JsonFileStoreSampleEntity, DiFixedStorePathProvider>();
        var container = builder.Build();

        // Assert
        var store = container.Resolve<IStore<JsonFileStoreSampleEntity>>();
        store.Should().BeOfType<JsonFileStore<JsonFileStoreSampleEntity>>();

        var pathProvider = container.Resolve<IStorePathProvider<JsonFileStoreSampleEntity>>();
        pathProvider.Should().BeOfType<DiFixedStorePathProvider>();

        var listableProvider = container.Resolve<IListableStorePathProvider<JsonFileStoreSampleEntity>>();
        listableProvider.Should().BeOfType<DiFixedStorePathProvider>();
    }

    [ Fact ]
    public void AddJsonFileStore_WhenPathProviderIsNotListable_DoesNotRegisterListablePathProvider()
    {
        // Arrange
        var builder = new ContainerBuilder();
        builder.AddJsonFileStore<JsonFileStoreSampleEntity, DiNonListableStorePathProvider>();
        var container = builder.Build();

        // Act
        var act = () => container.Resolve<IListableStorePathProvider<JsonFileStoreSampleEntity>>();

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>();
    }
    #endregion
}