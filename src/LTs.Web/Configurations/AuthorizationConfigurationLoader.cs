using LTs.Configurations.Extensions;
using LTs.Web.Authorization;
using Microsoft.Extensions.Configuration;

namespace LTs.Web.Configurations;

/// <summary>
///     Extensions to load <see cref="AuthorizationConfiguration" />
/// </summary>
public static class AuthorizationConfigurationLoader
{
    /// <summary>
    ///     Converts the configuration to a string.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The string representation of <see cref="AuthorizationConfiguration" />.</returns>
    public static AuthorizationConfiguration LoadAuthorizationConfiguration( this IConfiguration configuration )
    {
        var result = new AuthorizationConfiguration
        {
            AccessTokenUrl = configuration.GetRequiredValue<string>( nameof( AuthorizationConfiguration.AccessTokenUrl ) ),
            GrantType = configuration.GetRequiredValue<GrantType>( nameof( AuthorizationConfiguration.GrantType ) ),
            ClientId = configuration.GetRequiredValue<string>( nameof( AuthorizationConfiguration.ClientId ) ),
            ClientSecret = configuration.GetRequiredValue<string>( nameof( AuthorizationConfiguration.ClientSecret ) ),
            Scope = configuration.GetRequiredValue<string>( nameof( AuthorizationConfiguration.Scope ) )
        };

        return result;
    }
}