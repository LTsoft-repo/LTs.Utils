using LTs.DependencyInjections.Registrations;
using LTs.DependencyInjections.test.Infrastructure;
using LTs.TestUtils.FluentAssertions;
using LTs.DependencyInjections.test.Registrations.Infrastructure;

#pragma warning disable IDE0290

namespace LTs.DependencyInjections.test.Registrations;

public class RegistrationsLoaderTests : BaseTest
{
    public RegistrationsLoaderTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Discover
    [ Fact ]
    public void Discover_WithSampleRegistrations_ReturnsRegistrationsFromAssembly()
    {
        // Arrange
        var assembly = typeof( RegistrationsLoaderAssemblyAnchor ).Assembly;

        // Act
        var registrations = RegistrationsLoader.Discover( assembly );
        var registrationTypes = registrations.Select( registration => registration.GetType() ).ToArray();

        // Assert
        registrationTypes.Should().Contain( typeof( RegistrationsLoaderSampleServiceRegistration ) );
        registrationTypes.Should().Contain( typeof( RegistrationsLoaderDescriptorRegistration ) );
    }
    #endregion

    #region Apply
    [ Fact ]
    public void Apply_WithSampleRegistrations_RegistersDiscoveredServices()
    {
        // Arrange
        var assembly = typeof( RegistrationsLoaderAssemblyAnchor ).Assembly;
        var builder = new RecordingRegistrationBuilder();

        // Act
        RegistrationsLoader.Apply( assembly, builder );

        // Assert
        var singletons = builder.Singletons;
        singletons.Should().ContainExactlyEquivalent(
        [
            ( typeof( IService ), typeof( ServiceImplementation ) )
        ] );

        var descriptors = builder.Descriptors;
        descriptors.Should().ContainSingle();

        var descriptor = descriptors[ 0 ];
        descriptor.Should().BeEquivalentTo(
            new RegistrationDescriptorSample( typeof( IService ), typeof( ServiceImplementation ), "descriptor-tag" ) );
    }
    #endregion
}