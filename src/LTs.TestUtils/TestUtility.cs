using Microsoft.Extensions.Configuration;

namespace LTs.TestUtils;

/// <summary>
///     Utility methods for testing.
/// </summary>
public static class TestUtility
{
    /// <summary>
    ///     Gets the test configuration.
    ///     <para>
    ///         Adds: Environment variables and test settings files.
    ///     </para>
    /// </summary>
    /// <returns></returns>
    public static IConfiguration GetTestConfiguration() => new ConfigurationBuilder()
                                                           .AddEnvironmentVariables()
                                                           .AddTestSettings()
                                                           .Build();

    /// <summary>
    ///     Adds the test settings file to the configuration builder.
    /// </summary>
    /// <param name="builder">Configuration builder.</param>
    /// <returns>The Configuration builder.</returns>
    [ UsedImplicitly ]
    public static IConfigurationBuilder AddTestSettings( this IConfigurationBuilder builder )
    {
        // ReSharper disable StringLiteralTypo
        var configPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.UserProfile ),
                                       ".azurefunctions",
                                       "appsettings.tests.json" );
        // ReSharper restore StringLiteralTypo

        return builder.AddJsonFile( configPath, true );
    }

    /// <summary>
    ///     Retries the specified condition until it returns true or the timeout is reached.
    /// </summary>
    /// <param name="condition">The condition to check.</param>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <param name="pollingInterval">Polling interval in milliseconds.</param>
    /// <param name="userMessageCallback">A callback to provide a user-friendly message when the timeout is reached.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static async Task RetryAsync( Func<Task<bool>> condition,
                                         int timeout = 60 * 1000,
                                         int pollingInterval = 2 * 1000,
                                         Func<string>? userMessageCallback = null )
    {
        var start = DateTime.Now;

        while( !await condition() )
        {
            await Task.Delay( pollingInterval );

            //var shouldThrow = !Debugger.IsAttached || Debugger.IsAttached && throwWhenDebugging;

            //if( shouldThrow && ( DateTime.Now - start ).TotalMilliseconds > timeout )
            if( ( DateTime.Now - start ).TotalMilliseconds > timeout )
            {
                var error = "Condition not reached within timeout.";

                if( userMessageCallback != null )
                {
                    error += " " + userMessageCallback();
                }

                throw new ApplicationException( error );
            }
        }
    }
}