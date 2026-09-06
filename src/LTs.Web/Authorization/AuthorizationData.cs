namespace LTs.Web.Authorization;

/// <summary>
///     Represents the authorization data used to get an OAuth2 access token.
/// </summary>
public record AuthorizationData
{
    /// <summary>
    ///     The URL to get the access token from.
    /// </summary>
    [ UsedImplicitly ]
    public string AccessTokenUrl { get; init; } = "";

    /// <summary>
    ///     The grant type used to authenticate.
    /// </summary>
    [ UsedImplicitly ]
    public GrantType GrantType { get; init; }

    /// <summary>
    ///     The client ID.
    /// </summary>
    [ UsedImplicitly ]
    public string ClientId { get; init; } = "";

    /// <summary>
    ///     The client secret.
    /// </summary>
    [ UsedImplicitly ]
    public string Secret { get; init; } = "";

    /// <summary>
    ///     The scope the access token will be issued with.
    /// </summary>
    [ UsedImplicitly ]
    public string Scope { get; init; } = "";

    /// <summary>
    ///     The access token.
    /// </summary>
    [ UsedImplicitly ]
    public string AccessToken { get; init; } = "";

    /// <summary>
    ///     The expiration date of the access token.
    /// </summary>
    [ UsedImplicitly ]
    public DateTime ExpireAtUtc { get; init; }

    /// <summary>
    ///     Compare if two AuthorizationData objects are equal.
    ///     <para>
    ///         Uses the following properties to verify the equality:
    ///         <see cref="AccessTokenUrl" />,
    ///         <see cref="GrantType" />,
    ///         <see cref="ClientId" />,
    ///         <see cref="Secret" />,
    ///         <see cref="Scope" />,
    ///     </para>
    /// </summary>
    /// <param name="obj">The <see cref="AuthorizationData" /> to compare to.</param>
    /// <returns><c>true</c> if the objects are equal, <c>false</c> otherwise.</returns>
    public virtual bool Equals( AuthorizationData? obj )
    {
        if( obj is null )
        {
            return false;
        }

        return AccessTokenUrl == obj.AccessTokenUrl &&
               GrantType == obj.GrantType &&
               ClientId == obj.ClientId &&
               Secret == obj.Secret &&
               Scope == obj.Scope;
    }

    // ReSharper disable once GrammarMistakeInComment
    /// <summary>
    ///     Generates a hash code for the current object.
    ///     <para>
    ///         Uses the following properties to generate it:
    ///         <see cref="AccessTokenUrl" />,
    ///         <see cref="GrantType" />,
    ///         <see cref="ClientId" />,
    ///         <see cref="Secret" />,
    ///         <see cref="Scope" />,
    ///     </para>
    /// </summary>
    /// <returns>An <c>int</c> representing the hash code.</returns>
    public override int GetHashCode() => HashCode.Combine( AccessTokenUrl, GrantType, ClientId, Secret, Scope );
}