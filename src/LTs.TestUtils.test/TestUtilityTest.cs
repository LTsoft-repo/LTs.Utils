using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LTs.TestUtils.test;

public class TestUtilityTest
{
    #region GetTestConfiguration
    [ Fact ]
    public void GetTestConfiguration_WhenCalled_ReturnsConfiguration()
    {
        // Arrange
        Environment.SetEnvironmentVariable( "SomeEnvValue1", "Value1" );
        Environment.SetEnvironmentVariable( "SomeEnvValue2", "Value2" );

        // Act
        var configuration = TestUtility.GetTestConfiguration();

        // Assert
        configuration.Should().NotBeNull();
        configuration.GetSection( "SomeEnvValue1" ).Exists().Should().BeTrue();
        configuration.GetSection( "SomeEnvValue2" ).Exists().Should().BeTrue();

        configuration.GetSection( "SomeEnvValue1" ).Value.Should().Be( "Value1" );
        configuration.GetSection( "SomeEnvValue2" ).Value.Should().Be( "Value2" );
    }
    #endregion

    #region AddTestSettings
    [ Fact ]
    public void AddTestSettings_WhenCalled_ReturnsConfigurationBuilder()
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        builder.AddTestSettings();

        // Assert
        var sources = builder.Sources;
        sources.Should().NotBeEmpty();

        sources.Should().Contain( x => x.GetType() == typeof( JsonConfigurationSource ) )
               .Which.As<JsonConfigurationSource>()
               .Path.Should().Be( "appsettings.tests.json" );
    }
    #endregion

    #region RetryAsync
    [ Fact ]
    public async Task RetryAsync_WhenConditionIsTrue_DoesNotThrowException()
    {
        // Arrange
        Task<bool> Condition()
            => Task.FromResult( true );

        // Act
        await TestUtility.RetryAsync( Condition );

        // Assert
    }

    [ Fact ]
    public async Task RetryAsync_WhenConditionIsFalseAndTimeoutIsNotReached_ThrowsException()
    {
        // Arrange
        Task<bool> Condition()
            => Task.FromResult( false );

        // Act
        var act = () => TestUtility.RetryAsync( Condition, 1000, 100 );

        // Assert
        ( await act.Should().ThrowAsync<ApplicationException>() )
            .WithMessage( "Condition not reached within timeout." );
    }

    [ Fact ]
    public async Task RetryAsync_TakeMoreThanTimeout_ThrowsException()
    {
        // Arrange
        var startTime = DateTime.Now;
        var timeout = 300;
        var executionTime = 1000;

        Task<bool> Condition()
            => DateTime.Now < startTime.AddMilliseconds( executionTime ) ? Task.FromResult( false ) : Task.FromResult( true );

        // Act
        var watch = new Stopwatch();

        var act = async () =>
            {
                watch.Start();
                await TestUtility.RetryAsync( Condition, timeout, 100 );
                watch.Stop();
            };

        // Assert
        ( await act.Should().ThrowAsync<ApplicationException>() )
            .WithMessage( "Condition not reached within timeout." );

        watch.Stop();

        watch.ElapsedMilliseconds.Should().BeCloseTo( timeout, 150 );
    }
    #endregion
}