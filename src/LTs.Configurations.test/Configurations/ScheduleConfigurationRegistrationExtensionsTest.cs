using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using LTs.Configurations.Configurations;
using LTs.Configurations.Exceptions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Configurations;

public class ScheduleConfigurationRegistrationExtensionsTest
{
    #region AddScheduleConfiguration
    [ Fact ]
    public void AddScheduleConfiguration_ValidParameters_Successes()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration( "Schedules:Use1" );
        var container = builder.Build();

        // Assert
        container.IsRegistered<ScheduleConfiguration>().Should().BeTrue();

        var connectionString = container.Resolve<ScheduleConfiguration>();
        connectionString.Should().NotBeNull();
        connectionString.Should().BeEquivalentTo( new ScheduleConfiguration { TimeInMilliseconds = 100 } );
    }

    [ Fact ]
    public void AddScheduleConfiguration_WithoutConfiguration_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration( "Schedules:Use2" );
        var container = builder.Build();

        // Assert
        var act = () => container.Resolve<ScheduleConfiguration>();

        act.Should().Throw<DependencyResolutionException>()
           .WithInnerException<ConfigurationException>()
           .WithMessage( "Configuration parameter 'Schedules:Use2:TimeInMilliseconds' not defined." );
    }

    [ Fact ]
    public void AddScheduleConfiguration_WithoutEmptyValue_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration( "Schedules:Use1" );
        var container = builder.Build();

        // Assert
        var act = () => container.Resolve<ScheduleConfiguration>();

        act.Should().Throw<DependencyResolutionException>()
           .WithInnerException<InvalidOperationException>()
           .WithMessage( "Failed to convert configuration value* at 'Schedules:Use1:TimeInMilliseconds' to type 'System.Int32'." );
    }
    #endregion

    #region AddScheduleConfiguration<T>
    [ Fact ]
    public void AddScheduleConfigurationT_ValidParameters_Successes()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration<SomeClass>( "Schedules:Use1" );
        var container = builder.Build();

        // Assert
        container.IsRegistered<ScheduleConfiguration<SomeClass>>().Should().BeTrue();

        var connectionString = container.Resolve<ScheduleConfiguration<SomeClass>>();
        connectionString.Should().NotBeNull();
        connectionString.Should().BeEquivalentTo( new ScheduleConfiguration<SomeClass> { TimeInMilliseconds = 100 } );
    }

    [ Fact ]
    public void RegisterConnectionStringT_WithoutConfiguration_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration<SomeClass>( "Schedules:Use2" );
        var container = builder.Build();

        // Assert
        var act = () => container.Resolve<ScheduleConfiguration<SomeClass>>();

        act.Should().Throw<DependencyResolutionException>()
           .WithInnerException<ConfigurationException>()
           .WithMessage( "Configuration parameter 'Schedules:Use2:TimeInMilliseconds' not defined." );
    }

    [ Fact ]
    public void RegisterConnectionStringT_WithoutEmptyValue_Throws()
    {
        // Arrange
        var builder = new ContainerBuilder();

        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "" }
                            } )
                            .Build();

        builder.RegisterInstance<IConfiguration>( configuration );

        // Act
        builder.AddScheduleConfiguration<SomeClass>( "Schedules:Use1" );
        var container = builder.Build();

        // Assert
        var act = () => container.Resolve<ScheduleConfiguration<SomeClass>>();

        act.Should().Throw<DependencyResolutionException>()
           .WithInnerException<InvalidOperationException>()
           .WithMessage( "Failed to convert configuration value* at 'Schedules:Use1:TimeInMilliseconds' to type 'System.Int32'." );
    }
    #endregion

    [ UsedImplicitly ]
    // ReSharper disable once RedundantTypeDeclarationBody
    private class SomeClass { }
}
