namespace LTs.Web.Authorization;

/// <summary>
///     Represents the grant type of the token.
/// </summary>
public enum GrantType
{
    /// <summary>
    ///     The token will be issued using client credentials.
    /// </summary>
    ClientCredentials,

    /// <summary>
    ///     The token will be issued using username and password.
    /// </summary>
    Password,

    /// <summary>
    ///     The token will be issued using an authorization code.
    /// </summary>
    AuthorizationCode,

    /// <summary>
    ///     The token will be issued using a refresh token.
    /// </summary>
    RefreshToken,

    /// <summary>
    ///     The token will be issued using an implicit flow.
    /// </summary>
    Implicit,

    /// <summary>
    ///     The token will be issued using a SAML2 bearer token.
    /// </summary>
    Saml2Bearer,

    /// <summary>
    ///     The token will be issued using a JWT bearer token.
    /// </summary>
    JwtBearer,

    /// <summary>
    ///     The token will be issued using a device code.
    /// </summary>
    DeviceCode,

    /// <summary>
    ///     The token will be issued using a token exchange.
    /// </summary>
    TokenExchange,

    /// <summary>
    ///     The token will be issued using a CIBA flow.
    /// </summary>
    Ciba
}