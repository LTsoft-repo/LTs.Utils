using LTs.Web.Authorization;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0290

namespace LTs.Web.test.Infrastructure;

internal class TestHttpHandler : HttpHandler
{
    private readonly List<AuthorizationData> accessTokenCacheInternal = [ ];

    public TestHttpHandler( HttpClient httpClient, ILogger<TestHttpHandler> logger ) : base( httpClient, logger ) { }

    public TestHttpHandler( HttpClient httpClient, IEnumerable<AuthorizationData> cachedTokens, ILogger<TestHttpHandler> logger )
        : this( httpClient, logger )
        => accessTokenCacheInternal.AddRange( cachedTokens );

    public IEnumerable<AuthorizationData> GetCachedTokens()
        => accessTokenCacheInternal;

    public override Task<string> GetAccessTokenAsync( string accessTokenUrl,
                                                      GrantType grantType,
                                                      string clientId,
                                                      string secret,
                                                      string scope,
                                                      bool forceRefresh )
    {
        // Sets the Token cache with the values for this instance only.
        AccessTokenCache.Clear();
        AccessTokenCache.AddRange( accessTokenCacheInternal );

        var token = base.GetAccessTokenAsync( accessTokenUrl, grantType, clientId, secret, scope, forceRefresh );

        // Updates the internal Token cache.
        accessTokenCacheInternal.Clear();
        accessTokenCacheInternal.AddRange( AccessTokenCache );

        return token;
    }
}