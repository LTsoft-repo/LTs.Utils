using Autofac;
using LTs.DependencyInjections.Abstractions;
using LTs.DependencyInjections.Registrations;
using LTs.DependencyInjections.test.Infrastructure;
using LTs.DependencyInjections.test.Registrations.Infrastructure;

#pragma warning disable IDE0290

namespace LTs.DependencyInjections.test.Registrations;

public class RegistrationBuilderTests : BaseTest
{
    public RegistrationBuilderTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region RegisterSingleton
    [ Fact ]
    public void RegisterSingleton_WithServiceImplementation_RegistersSingleInstance()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        IRegistrationBuilder registrationBuilder = new RegistrationBuilder( containerBuilder );

        // Act
        registrationBuilder.RegisterSingleton<IService, ServiceImplementation>();
        var container = containerBuilder.Build();

        // Assert
        var first = container.Resolve<IService>();
        var second = container.Resolve<IService>();
        first.Should().BeSameAs( second );
    }
    #endregion

    #region RegisterDescriptor
    [ Fact ]
    public void RegisterDescriptor_WithRegistrationDescriptorSample_RegistersResolvableInstance()
    {
        // Arrange
        var containerBuilder = new ContainerBuilder();
        IRegistrationBuilder registrationBuilder = new RegistrationBuilder( containerBuilder );
        var descriptor = new RegistrationDescriptorSample( typeof( IService ), typeof( ServiceImplementation ), "test-tag" );

        // Act
        registrationBuilder.RegisterDescriptor( descriptor );
        var container = containerBuilder.Build();

        // Assert
        var resolved = container.Resolve<IRegistrationDescriptor>();
        resolved.Should().BeEquivalentTo(
            new RegistrationDescriptorSample( typeof( IService ), typeof( ServiceImplementation ), "test-tag" ) );
    }
    #endregion
}