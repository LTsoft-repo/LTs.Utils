using System.Collections.Immutable;
using IdentityModel;

namespace LTs.Web.Authorization;

/// <summary>
///     Constants with string representations of grant types.
/// </summary>
public static class GrantTypes
{
    /// <summary>
    ///     The client credentials grant types string representation.
    /// </summary>
    public static readonly ImmutableDictionary<int, string> StringRepresentation =
        new Dictionary<int, string>
        {
            [ (int)GrantType.ClientCredentials ] = OidcConstants.GrantTypes.ClientCredentials,
            [ (int)GrantType.Password ] = OidcConstants.GrantTypes.Password,
            [ (int)GrantType.AuthorizationCode ] = OidcConstants.GrantTypes.AuthorizationCode,
            [ (int)GrantType.RefreshToken ] = OidcConstants.GrantTypes.RefreshToken,
            [ (int)GrantType.Implicit ] = OidcConstants.GrantTypes.Implicit,
            [ (int)GrantType.Saml2Bearer ] = OidcConstants.GrantTypes.Saml2Bearer,
            [ (int)GrantType.JwtBearer ] = OidcConstants.GrantTypes.JwtBearer,
            [ (int)GrantType.DeviceCode ] = OidcConstants.GrantTypes.DeviceCode,
            [ (int)GrantType.TokenExchange ] = OidcConstants.GrantTypes.TokenExchange,
            [ (int)GrantType.Ciba ] = OidcConstants.GrantTypes.Ciba
        }.ToImmutableDictionary();
}