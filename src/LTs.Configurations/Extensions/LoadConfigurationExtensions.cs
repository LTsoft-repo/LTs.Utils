using LTs.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace LTs.Configurations.Extensions;

/// <summary>
///     Extensions to load the default configuration for an assembly.
/// </summary>
public static class LoadConfigurationExtensions
{
    /// <summary>
    ///     Add the default configuration for the assembly containing the type <typeparamref name="T" />.
    ///     <para>
    ///         Loads the configuration in the following order:
    ///         <list type="bullet">
    ///             <item>appsettings.json</item>
    ///             <item>appsettings.&lt;Environment&gt;.json</item>
    ///             <item>User Secrets</item>
    ///             <item>Key per File</item>
    ///             <item>Environment Variables</item>
    ///             <item>Additional configuration (if any)</item>
    ///         </list>
    ///     </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder">The configuration builder.</param>
    /// <param name="additionalConfigurationAction">Additional configuration action.</param>
    /// <returns>The configuration builder with the added configurations.</returns>
    [ UsedImplicitly ]
    public static IConfigurationBuilder AddDefaultConfigurationForAssembly<T>( this IConfigurationBuilder builder,
                                                                               Action<IConfigurationBuilder>? additionalConfigurationAction )
        where T : class
    {
        var environmentName = EnvironmentUtils.GetEnvironmentName();

        builder
            .AddJsonFile( "appsettings.json", true, false )
            .AddJsonFile( $"appsettings.{environmentName}.json", true, false )
            .AddUserSecrets<T>( true, false )
            .ParseEmptyString( b => b.AddKeyPerFile( source =>
                {
                    const string directoryPath = "/mnt/secret-store";

                    if( Directory.Exists( directoryPath ) )
                    {
                        source.FileProvider = new PhysicalFileProvider( directoryPath );
                    }

                    source.Optional = true;
                    source.ReloadOnChange = false;
                    source.SectionDelimiter = "--";
                } ) )
            .AddEnvironmentVariables();

        additionalConfigurationAction?.Invoke( builder );

        return builder;
    }
}