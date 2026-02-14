using LTs.Web.Authorization;

namespace LTs.Web.test.Authorization;

public class AuthorizationDataTest
{
    [ Fact ]
    public void Equals_SameAuthenticationInfo_ReturnsTrue()
    {
        // Arrange
        var firstAuthorizationData = new AuthorizationData
        {
            AccessTokenUrl = "https://example.com/api",
            GrantType = GrantType.ClientCredentials,
            ClientId = "clientId",
            Secret = "secret",
            Scope = "scope"
        };

        var secondAuthorizationData = new AuthorizationData
        {
            AccessTokenUrl = "https://example.com/api",
            GrantType = GrantType.ClientCredentials,
            ClientId = "clientId",
            Secret = "secret",
            Scope = "scope"
        };

        // Act
        var result = firstAuthorizationData.Equals( secondAuthorizationData );

        // Assert
        result.Should().BeTrue();
    }

    [ Fact ]
    public void Equals_DifferentToken_ReturnsTrue()
    {
        // Arrange
        var firstAuthorizationData = new AuthorizationData
        {
            AccessTokenUrl = "https://example.com/api",
            GrantType = GrantType.ClientCredentials,
            ClientId = "clientId",
            Secret = "secret",
            Scope = "scope",
            AccessToken = "accessToken",
            ExpireAtUtc = DateTime.Parse( "2022-01-01 12:33:11" )
        };

        var secondAuthorizationData = firstAuthorizationData with
        {
            AccessToken = "accessToken2",
            ExpireAtUtc = DateTime.Parse( "2022-03-01 12:33:11" )
        };

        // Act
        var result = firstAuthorizationData.Equals( secondAuthorizationData );

        // Assert
        result.Should().BeTrue();
    }

    [ Fact ]
    public void Equals_NullObject_ReturnsFalse()
    {
        // Arrange
        var authorizationData = new AuthorizationData();
        AuthorizationData? obj = null;

        // Act
        var result = authorizationData.Equals( obj );

        // Assert
        result.Should().BeFalse();
    }

    [ Fact ]
    public void Equals_DifferentAuthenticationInfo_ReturnsFalse()
    {
        // Arrange
        var firstAuthorizationData = new AuthorizationData
        {
            AccessTokenUrl = "https://example.com/api",
            GrantType = GrantType.ClientCredentials,
            ClientId = "clientId",
            Secret = "secret",
            Scope = "scope"
        };

        var secondAuthorizationData = firstAuthorizationData with
        {
            AccessTokenUrl = "https://example2.com/api"
        };

        // Act
        var result = firstAuthorizationData.Equals( secondAuthorizationData );

        // Assert
        result.Should().BeFalse();
    }

    [ Fact ]
    public void Equals_DifferentGrantType_ReturnsFalse()
    {
        // Arrange
        var firstAuthorizationData = new AuthorizationData
        {
            AccessTokenUrl = "https://example.com/api",
            GrantType = GrantType.ClientCredentials,
            ClientId = "clientId",
            Secret = "secret",
            Scope = "scope"
        };

        var secondAuthorizationData = firstAuthorizationData with
        {
            GrantType = GrantType.Password
        };

        // Act
        var result = firstAuthorizationData.Equals( secondAuthorizationData );

        // Assert
        result.Should().BeFalse();
    }
}