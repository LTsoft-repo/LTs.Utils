using Microsoft.Extensions.Configuration;

namespace LTs.TestUtils.Configurations;

/// <summary>
///     Configuration manager for tests.
/// </summary>
public class ConfigurationManager
{
    private readonly Type referenceType;

    /// <summary>
    ///     Loaded configuration.
    /// </summary>
    public IConfiguration? Configuration { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationManager" /> class.
    /// </summary>
    public ConfigurationManager( Type referenceType )
    {
        this.referenceType = referenceType;
        LoadConfiguration();
    }

    private void LoadConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder()
                                   .SetBasePath( Directory.GetCurrentDirectory() )
                                   .AddJsonFile( "appsettings.json", true, true )
                                   .AddUserSecrets( referenceType.Assembly, true, true );

        Configuration = configurationBuilder.Build();
    }
}