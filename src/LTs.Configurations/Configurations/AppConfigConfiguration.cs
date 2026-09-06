namespace LTs.Configurations.Configurations;

/// <summary>
///     Default configuration class for the application.
/// </summary>
public record AppConfigConfiguration
{
    /// <summary>
    ///     Gets or sets the connection string for the application's configuration.
    /// </summary>
    public string? ConnectionString { get; init; }
}
