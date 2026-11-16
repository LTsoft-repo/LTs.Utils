using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.Extensions;
using LTs.DependencyInjections.test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Extensions;

public class AutofacPopulateExtensionsTest
{
    #region Populate
    [ Fact ]
    public void Populate_WithTypeRegisteredInContainer_Successes()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterType<SomeClass>();
        var container = containerBuilder.Build();

        var builder = new ContainerBuilder();

        // Act
        var result = builder.Populate( container );
        var resultContainer = result.Build();

        // Assert
        resultContainer.IsRegistered<SomeClass>().Should().BeTrue();
    }

    [ Fact ]
    public void Populate_WithEmptyContainer_Successes()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();

        var builder = new ContainerBuilder();

        // Act
        var result = builder.Populate( container );
        var act = () => result.Build();

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void Populate_WithKeyedSingleton_ResolvesViaKeyedServiceProvider()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, BlobStorage>( "blob" );
        services.AddKeyedSingleton<IStorage, FileStorage>( "file" );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var blob = serviceProvider.GetRequiredKeyedService<IStorage>( "blob" );
        var file = serviceProvider.GetRequiredKeyedService<IStorage>( "file" );

        // Assert
        blob.Should().BeOfType<BlobStorage>();
        blob.Name.Should().Be( "blob" );

        file.Should().BeOfType<FileStorage>();
        file.Name.Should().Be( "file" );
    }

    [ Fact ]
    public void Populate_WithMicrosoftAnyKey_ResolvesForConcreteKey()
    {
        // Arrange
        const string resolveKey = "custom-key";

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, AnyKeyStorage>( KeyedService.AnyKey );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var storage = serviceProvider.GetRequiredKeyedService<IStorage>( resolveKey );

        // Assert
        storage.Should().BeOfType<AnyKeyStorage>();

        var anyKeyStorage = (AnyKeyStorage)storage;
        anyKeyStorage.ResolvedKey.Should().Be( resolveKey );
    }

    [ Fact ]
    public void Populate_WithFromKeyedServices_ResolvesKeyedDependencyInConstructor()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, BlobStorage>( "blob" );
        services.AddKeyedSingleton<IStorage, FileStorage>( "file" );
        services.AddTransient<FromKeyedServicesConsumer>();

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var consumer = serviceProvider.GetRequiredService<FromKeyedServicesConsumer>();

        // Assert
        consumer.Storage.Should().BeOfType<BlobStorage>();
        consumer.Storage.Name.Should().Be( "blob" );
    }
    #endregion
}