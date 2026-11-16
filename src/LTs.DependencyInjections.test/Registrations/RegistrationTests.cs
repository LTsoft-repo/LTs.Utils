using LTs.DependencyInjections.Abstractions;
using LTs.DependencyInjections.test.Infrastructure;
using LTs.TestUtils.FluentAssertions;
using LTs.DependencyInjections.test.Registrations.Infrastructure;

#pragma warning disable IDE0290

namespace LTs.DependencyInjections.test.Registrations;

public class RegistrationTests : BaseTest
{
    public RegistrationTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Register
    [ Fact ]
    public void Register_WithSampleServiceRegistration_RegistersSingletonService()
    {
        // Arrange
        IRegistration registration = new RegistrationsLoaderSampleServiceRegistration();
        var builder = new RecordingRegistrationBuilder();

        // Act
        registration.Register( builder );

        // Assert
        var singletons = builder.Singletons;
        singletons.Should().ContainExactlyEquivalent(
        [
            ( typeof( IService ), typeof( ServiceImplementation ) )
        ] );
    }
    #endregion
}