using System.Diagnostics;

namespace LTs.TestUtils;

/// <summary>
///     Set of methods to Wait for a condition to be satisfied.
/// </summary>
public static class Wait
{
    /// <summary>
    ///     Waits an async condition to be satisfied.
    /// </summary>
    /// <param name="condition">Condition to be satisfied.</param>
    /// <param name="timeout">Timeout to wait for the condition.</param>
    /// <returns></returns>
    public static async Task ForAsync( Func<Task<bool>> condition, TimeSpan timeout )
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        while( true )
        {
            // Checks condition
            if( await condition() )
            {
                return;
            }

            // Checks timeout
            if( stopwatch.Elapsed >= timeout )
            {
                break;
            }

            await Task.Delay( 10 ); // This used to be 100ms
        }

        if( stopwatch.Elapsed > timeout )
        {
            throw new Exception( "Condition not satisfied in given time." );
        }
    }

    /// <summary>
    ///     Waits an async condition to be satisfied
    /// </summary>
    /// <param name="condition">Condition to be satisfied.</param>
    /// <param name="timeout">Timeout to wait for the condition.</param>
    /// <returns></returns>
    public static async Task ForAsync( Func<bool> condition, TimeSpan timeout ) =>
        await ForAsync( () => Task.FromResult( condition() ), timeout );
}