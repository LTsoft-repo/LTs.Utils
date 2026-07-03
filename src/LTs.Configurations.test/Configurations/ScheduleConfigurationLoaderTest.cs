using JetBrains.Annotations;
using LTs.Configurations.Configurations;
using LTs.Configurations.Exceptions;
using Microsoft.Extensions.Configuration;

namespace LTs.Configurations.test.Configurations;

public class ScheduleConfigurationLoaderTest
{
    #region LoadScheduleConfiguration
    [ Fact ]
    public void LoadScheduleConfiguration_ValidParameters_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use1" );

        // Act
        var result = section.LoadScheduleConfiguration();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo( new ScheduleConfiguration { TimeInMilliseconds = 100 } );
    }

    [ Fact ]
    public void LoadScheduleConfiguration_WithoutConfiguration_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use2" );

        // Act
        var act = () => section.LoadScheduleConfiguration();

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'Schedules:Use2:TimeInMilliseconds' not defined." );
    }

    [ Fact ]
    public void LoadScheduleConfiguration_WithoutEmptyValue_Throws()
    {
        // Arrange
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use1" );

        // Act
        var act = () => section.LoadScheduleConfiguration();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage( "Failed to convert configuration value* at 'Schedules:Use1:TimeInMilliseconds' to type 'System.Int32'." );
    }
    #endregion

    #region LoadScheduleConfiguration<T>
    [ Fact ]
    public void LoadScheduleConfigurationT_ValidParameters_Successes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use1" );

        // Act
        var result = section.LoadScheduleConfiguration<SomeClass>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo( new ScheduleConfiguration<SomeClass> { TimeInMilliseconds = 100 } );
    }

    [ Fact ]
    public void LoadScheduleConfigurationT_WithoutConfiguration_Throws()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "100" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use2" );

        // Act
        var act = () => section.LoadScheduleConfiguration<SomeClass>();

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'Schedules:Use2:TimeInMilliseconds' not defined." );
    }

    [ Fact ]
    public void LoadScheduleConfigurationT_WithoutEmptyValue_Throws()
    {
        // Arrange
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Schedules:Use1:TimeInMilliseconds", "" }
                            } )
                            .Build();

        var section = configuration.GetSection( "Schedules:Use1" );

        // Act
        var act = () => section.LoadScheduleConfiguration<SomeClass>();

        // Assert
        act.Should().Throw<InvalidOperationException>()
           .WithMessage( "Failed to convert configuration value* at 'Schedules:Use1:TimeInMilliseconds' to type 'System.Int32'." );
    }
    #endregion

    [ UsedImplicitly ]
    // ReSharper disable once RedundantTypeDeclarationBody
    private class SomeClass { }
}
