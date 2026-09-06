using LTs.Web.Authorization;

namespace LTs.Web.Configurations;

/// <summary>
///     Represents the configuration to be used to request an Access token.
/// </summary>
public record AuthorizationConfiguration
{
    /// <summary>
    ///     The URL to get the access token from.
    /// </summary>
    public string AccessTokenUrl { get; init; } = "";

    /// <summary>
    ///     The grant type used to authenticate.
    /// </summary>
    public GrantType GrantType { get; init; }

    /// <summary>
    ///     The client ID, or username.
    /// </summary>
    public string ClientId { get; init; } = "";

    /// <summary>
    ///     The client secret, or password.
    /// </summary>
    public string ClientSecret { get; init; } = "";

    /// <summary>
    ///     The scope the access token will be issued with.
    /// </summary>
    public string Scope { get; init; } = "";
}