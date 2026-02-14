using System.Diagnostics;
using FluentAssertions.Extensions;
using LTs.TestUtils.Loggers;
using LTs.TestUtils.Tests;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290
#pragma warning disable IDE0039

namespace LTs.TestUtils.test;

public class WaitTest : DisposableTest
{
    public WaitTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ForAsync (with Task condition)
    [ Fact ]
    public async Task ForAsync_AwaitableCondition_Success()
    {
        // Arrange
        var flag = false;
        var stopwatch = new Stopwatch();

        var backgroundTask = async () =>
            {
                await Task.Delay( 100 );
                flag = true;
            };

        // Act
        stopwatch.Start();
#pragma warning disable CS4014 // Does not wait for the task to be completed because is used in a fire-and-forget manner, but in this case is intentional.
        Task.Run( backgroundTask );
#pragma warning restore CS4014

        await Wait.ForAsync( () => Task.FromResult( flag ),
                             200.Milliseconds() );

        stopwatch.Stop();

        // Assert
        TestOutput.WriteLine( $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms" );
        flag.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeCloseTo( 100, 100 );
    }

    [ Fact ]
    public async Task ForAsync_AwaitableCondition_Timeout()
    {
        // Arrange
        var flag = false;
        var stopwatch = new Stopwatch();
        var logger = new TestLogger( TestOutput );

        var backgroundTask = async () =>
            {
                await Task.Delay( 500 );
                flag = true;
                logger.LogInformation( "Background task completed." );
            };

        Exception? exception = null;

        // Act
        stopwatch.Start();
        logger.LogInformation( "Starting background task..." );
#pragma warning disable CS4014 // Does not wait for the task to be completed because is used in a fire-and-forget manner, but in this case is intentional.
        Task.Run( backgroundTask );
#pragma warning restore CS4014

        try
        {
            await Wait.ForAsync( () => Task.FromResult( flag ),
                                 100.Milliseconds() );

            logger.LogInformation( "Wait completed without exception." );
        }
        catch( Exception ex )
        {
            exception = ex;
            logger.LogInformation( "Caught exception: {Message}", ex.Message );
        }

        // Assert
        logger.LogInformation( "Asserting..." );

        exception.Should().NotBeNull();
        exception.Should().BeOfType<Exception>();
        exception!.Message.Should().Be( "Condition not satisfied in given time." );
    }

    [ Fact ]
    public async Task ForAsync_SynchronousCondition_Success()
    {
        // Arrange
        var flag = false;
        var stopwatch = new Stopwatch();

        var backgroundTask = async () =>
            {
                await Task.Delay( 100 );
                flag = true;
            };

        // Act
        stopwatch.Start();
#pragma warning disable CS4014 // Does not wait for the task to be completed because is used in a fire-and-forget manner, but in this case is intentional.
        Task.Run( backgroundTask );
#pragma warning restore CS4014

        await Wait.ForAsync( () => flag,
                             250.Milliseconds() );

        stopwatch.Stop();

        // Assert
        flag.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeInRange( 100, 260 );
    }

    [ Fact ]
    public async Task ForAsync_SynchronousCondition_Timeout()
    {
        // Arrange
        var flag = false;
        var stopwatch = new Stopwatch();

        var backgroundTask = async () =>
            {
                await Task.Delay( 150 );
                flag = true;
            };

        // Act
        stopwatch.Start();
#pragma warning disable CS4014 // Does not wait for the task to be completed because is used in a fire-and-forget manner, but in this case is intentional.
        Task.Run( backgroundTask );
#pragma warning restore CS4014

        var act = async () => await Wait.ForAsync( () => Task.FromResult( flag ),
                                                   100.Milliseconds() );

        // Assert
        await act.Should().ThrowAsync<Exception>()
                 .WithMessage( "Condition not satisfied in given time." );
    }
    #endregion

    #region ForAsync (with Fucntion condition)
    [ Fact ]
    public async Task ForAsync_Condition_Success()
    {
        // Arrange
        var flag = false;
        var stopwatch = new Stopwatch();

        var backgroundTask = async () =>
            {
                await Task.Delay( 100 );
                flag = true;
            };

        // Act
        stopwatch.Start();
#pragma warning disable CS4014 // Does not wait for the task to be completed because is used in a fire-and-forget manner, but in this case is intentional.
        Task.Run( backgroundTask );
#pragma warning restore CS4014

        await Wait.ForAsync( () => flag,
                             200.Milliseconds() );

        stopwatch.Stop();

        // Assert
        flag.Should().BeTrue();
        stopwatch.ElapsedMilliseconds.Should().BeCloseTo( 100, 100 );
    }
    #endregion
}