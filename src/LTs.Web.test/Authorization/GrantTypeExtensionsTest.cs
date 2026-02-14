using LTs.Web.Authorization;

namespace LTs.Web.test.Authorization;

public class GrantTypeExtensionsTest
{
    #region ToIdentityString
    [ Theory ]
    [ InlineData( GrantType.Password, "password" ) ]
    [ InlineData( GrantType.AuthorizationCode, "authorization_code" ) ]
    [ InlineData( GrantType.ClientCredentials, "client_credentials" ) ]
    [ InlineData( GrantType.RefreshToken, "refresh_token" ) ]
    [ InlineData( GrantType.Implicit, "implicit" ) ]
    [ InlineData( GrantType.Saml2Bearer, "urn:ietf:params:oauth:grant-type:saml2-bearer" ) ]
    [ InlineData( GrantType.JwtBearer, "urn:ietf:params:oauth:grant-type:jwt-bearer" ) ]
    [ InlineData( GrantType.DeviceCode, "urn:ietf:params:oauth:grant-type:device_code" ) ]
    [ InlineData( GrantType.TokenExchange, "urn:ietf:params:oauth:grant-type:token-exchange" ) ]
    [ InlineData( GrantType.Ciba, "urn:openid:params:grant-type:ciba" ) ]
    public void ToIdentityString_WithClientCredentials_ReturnsClientCredentials( GrantType grantType, string expected )
    {
        // Arrange
        // Act
        var result = grantType.ToIdentityString();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToIdentityString_WithUnknownGrantType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var grantType = (GrantType)99;

        // Act
        Action act = () => grantType.ToIdentityString();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    #endregion

    #region ToGrantType
    [ Theory ]
    [ InlineData( "password", GrantType.Password ) ]
    [ InlineData( "authorization_code", GrantType.AuthorizationCode ) ]
    [ InlineData( "client_credentials", GrantType.ClientCredentials ) ]
    [ InlineData( "refresh_token", GrantType.RefreshToken ) ]
    [ InlineData( "implicit", GrantType.Implicit ) ]
    [ InlineData( "urn:ietf:params:oauth:grant-type:saml2-bearer", GrantType.Saml2Bearer ) ]
    [ InlineData( "urn:ietf:params:oauth:grant-type:jwt-bearer", GrantType.JwtBearer ) ]
    [ InlineData( "urn:ietf:params:oauth:grant-type:device_code", GrantType.DeviceCode ) ]
    [ InlineData( "urn:ietf:params:oauth:grant-type:token-exchange", GrantType.TokenExchange ) ]
    [ InlineData( "urn:openid:params:grant-type:ciba", GrantType.Ciba ) ]
    public void ToGrantType_WithClientCredentials_ReturnsClientCredentials( string grantType, GrantType expected )
    {
        // Arrange
        // Act
        var result = grantType.ToGrantType();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToGrantType_WithUnknownGrantType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var grantType = "unknown";

        // Act
        Action act = () => grantType.ToGrantType();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    #endregion
}