using Autofac;
using Autofac.Core.Registration;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.Extensions;
using LTs.DependencyInjections.test.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.DependencyInjection;

public class AutofacServiceProviderTest : BaseTest
{
    public AutofacServiceProviderTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetService
    [ Fact ]
    public void GetService_WhenRegistered_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var result = serviceProvider.GetService( typeof( SomeClass ) );

        // Assert
        result.Should().BeOfType<SomeClass>();
    }

    [ Fact ]
    public void GetService_WhenNotRegistered_ReturnsNull()
    {
        // Arrange
        var builder = new ContainerBuilder();
        builder.Populate( new ServiceCollection() );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var result = serviceProvider.GetService( typeof( SomeClass ) );

        // Assert
        result.Should().BeNull();
    }
    #endregion

    #region GetRequiredService
    [ Fact ]
    public void GetRequiredService_WhenRegistered_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var result = serviceProvider.GetRequiredService( typeof( SomeClass ) );

        // Assert
        result.Should().BeOfType<SomeClass>();
    }
    #endregion

    #region GetKeyedService
    [ Fact ]
    public void GetKeyedService_WithNullKey_ResolvesUnkeyedRegistration()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var result = serviceProvider.GetKeyedService( typeof( SomeClass ), null );

        // Assert
        result.Should().BeOfType<SomeClass>();
    }

    [ Fact ]
    public void GetRequiredKeyedService_WhenNotRegistered_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();
        builder.Populate( new ServiceCollection() );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var act = () => serviceProvider.GetRequiredKeyedService( typeof( IStorage ), "missing" );

        // Assert
        act.Should().Throw<ComponentNotRegisteredException>();
    }

    [ Fact ]
    public void GetRequiredKeyedService_WhenServiceKeyTypeConverterIsInvalid_ThrowsInvalidOperationException()
    {
        // Arrange
        const string serviceKey = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee";

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, InvalidServiceKeyTypeConverterStorage>( serviceKey );

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var act = () => serviceProvider.GetRequiredKeyedService( typeof( IStorage ), serviceKey );

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithInnerException<Autofac.Extensions.DependencyInjection.KeyTypeConversionException>();
    }

    [ Fact ]
    public void GetRequiredKeyedService_WhenServiceKeyConversionFails_ThrowsInvalidOperationException()
    {
        // Arrange
        const int serviceKey = 42;

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, GuidServiceKeyStorageNoConverter>( serviceKey );

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var act = () => serviceProvider.GetRequiredKeyedService( typeof( IStorage ), serviceKey );

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithInnerException<Autofac.Extensions.DependencyInjection.KeyTypeConversionException>();
    }

    [ Fact ]
    public void GetKeyedService_WhenServiceKeyConversionFails_ThrowsInvalidOperationException()
    {
        // Arrange
        const int serviceKey = 42;

        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, GuidServiceKeyStorageNoConverter>( serviceKey );

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var act = () => serviceProvider.GetKeyedService( typeof( IStorage ), serviceKey );

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithInnerException<Autofac.Extensions.DependencyInjection.KeyTypeConversionException>();
    }

    [ Fact ]
    public void GetKeyedService_WhenNotRegistered_ReturnsNull()
    {
        // Arrange
        var builder = new ContainerBuilder();
        builder.Populate( new ServiceCollection() );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var result = serviceProvider.GetKeyedService( typeof( IStorage ), "missing" );

        // Assert
        result.Should().BeNull();
    }
    #endregion

    #region IsService
    [ Fact ]
    public void IsService_WhenRegistered_ReturnsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var isService = serviceProvider.IsService( typeof( SomeClass ) );

        // Assert
        isService.Should().BeTrue();
    }

    [ Fact ]
    public void IsKeyedService_WithNullKey_MatchesIsService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var isKeyedService = serviceProvider.IsKeyedService( typeof( SomeClass ), null );
        var isService = serviceProvider.IsService( typeof( SomeClass ) );

        // Assert
        isKeyedService.Should().Be( isService );
    }

    [ Fact ]
    public void IsKeyedService_WithRegisteredKey_ReturnsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IStorage, BlobStorage>( "blob" );

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var serviceProvider = new AutofacServiceProvider( builder.Build() );

        // Act
        var isKeyedService = serviceProvider.IsKeyedService( typeof( IStorage ), "blob" );

        // Assert
        isKeyedService.Should().BeTrue();
    }
    #endregion

    #region Dispose
    [ Fact ]
    public void Dispose_DisposesLifetimeScope()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );

        // Act
        serviceProvider.Dispose();
        var act = () => container.Resolve<SomeClass>();

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }

    [ Fact ]
    public async Task DisposeAsync_DisposesLifetimeScope()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<SomeClass>();

        var builder = new ContainerBuilder();
        builder.Populate( services );
        var container = builder.Build();
        var serviceProvider = new AutofacServiceProvider( container );

        // Act
        await serviceProvider.DisposeAsync();
        var act = () => container.Resolve<SomeClass>();

        // Assert
        act.Should().Throw<ObjectDisposedException>();
    }
    #endregion
}