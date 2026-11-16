using Autofac;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.Extensions;
using LTs.DependencyInjections.test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Extensions;

public class AutofacPopulateExtensionsTest : BaseTest
{
    public AutofacPopulateExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

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

    [ Fact ]
    public void Populate_WithNullDescriptors_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new ContainerBuilder();

        // Act
        var act = () => builder.Populate( (IEnumerable<ServiceDescriptor>)null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName( "descriptors" );
    }

    [ Fact ]
    public void Populate_WithImplementationFactory_ResolvesService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IService>( _ => new ServiceImplementation() );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var service = serviceProvider.GetRequiredService<IService>();

        // Assert
        service.Should().BeOfType<ServiceImplementation>();
    }

    [ Fact ]
    public void Populate_WithKeyedImplementationFactory_ResolvesService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage>( "factory-key", ( _, key ) => new KeyedFactoryStorage( key! ) );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var storage = serviceProvider.GetRequiredKeyedService<IStorage>( "factory-key" );

        // Assert
        storage.Should().BeOfType<KeyedFactoryStorage>();
        storage.Name.Should().Be( "factory-key" );
    }

    [ Fact ]
    public void Populate_WithImplementationInstance_ResolvesSameInstance()
    {
        // Arrange
        var instance = new SomeClass();
        var services = new ServiceCollection();
        services.AddSingleton( instance );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var resolved = serviceProvider.GetRequiredService<SomeClass>();

        // Assert
        resolved.Should().BeSameAs( instance );
    }

    [ Fact ]
    public void Populate_WithOpenGenericRegistration_ResolvesClosedGeneric()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton( typeof( ITypedService<> ), typeof( TypedServiceImplementation<> ) );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var service = serviceProvider.GetRequiredService( typeof( ITypedService<int> ) );

        // Assert
        service.Should().BeOfType<TypedServiceImplementation<int>>();
    }

    [ Fact ]
    public void Populate_WithServiceKeyAttribute_ConvertsKeyToConstructorParameterType()
    {
        // Arrange
        const int serviceKey = 42;

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, StringServiceKeyStorage>( serviceKey );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var storage = serviceProvider.GetRequiredKeyedService<IStorage>( serviceKey );

        // Assert
        storage.Should().BeOfType<StringServiceKeyStorage>();

        var stringKeyStorage = (StringServiceKeyStorage)storage;
        stringKeyStorage.ResolvedKey.Should().Be( "42" );
    }

    [ Fact ]
    public void Populate_WithServiceKeyAttribute_ConvertsStringKeyViaTryParse()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, IntServiceKeyStorage>( "42" );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var storage = serviceProvider.GetRequiredKeyedService<IStorage>( "42" );

        // Assert
        storage.Should().BeOfType<IntServiceKeyStorage>();

        var intKeyStorage = (IntServiceKeyStorage)storage;
        intKeyStorage.ResolvedKey.Should().Be( 42 );
    }

    [ Fact ]
    public void Populate_WithServiceKeyAttribute_UsesTypeConverterOnParameter()
    {
        // Arrange
        const string serviceKey = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee";
        var expectedKey = Guid.Parse( serviceKey );

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, GuidServiceKeyStorage>( serviceKey );

        var builder = new ContainerBuilder();

        // Act
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );
        var storage = serviceProvider.GetRequiredKeyedService<IStorage>( serviceKey );

        // Assert
        storage.Should().BeOfType<GuidServiceKeyStorage>();

        var guidKeyStorage = (GuidServiceKeyStorage)storage;
        guidKeyStorage.ResolvedKey.Should().Be( expectedKey );
    }

    [ Fact ]
    public void Populate_WithScopedService_ResolvesWithinCreatedScope()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var container = builder.Build();
        var scopeFactory = container.Resolve<IServiceScopeFactory>();

        // Act
        using var scope = scopeFactory.CreateScope();
        var fromScope = scope.ServiceProvider.GetRequiredService<SomeClass>();

        // Assert
        fromScope.Should().BeOfType<SomeClass>();
    }

    [ Fact ]
    public async Task Populate_WithScopedService_DisposeScopeAsync_DisposesScope()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var container = builder.Build();
        var scopeFactory = container.Resolve<IServiceScopeFactory>();
        var scope = scopeFactory.CreateScope();
        scope.ServiceProvider.GetRequiredService<SomeClass>();

        // Act
        if( scope is IAsyncDisposable asyncScope )
        {
            await asyncScope.DisposeAsync();
        }

        // Assert
        scope.Should().BeAssignableTo<IAsyncDisposable>();
    }
    #endregion
}