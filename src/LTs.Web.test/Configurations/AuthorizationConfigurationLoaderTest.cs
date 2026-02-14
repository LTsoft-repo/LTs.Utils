using LTs.Configurations.Exceptions;
using LTs.Web.Authorization;
using LTs.Web.Configurations;
using Microsoft.Extensions.Configuration;

namespace LTs.Web.test.Configurations;

public class AuthorizationConfigurationLoaderTest
{
    #region LoadAuthorizationConfiguration
    [ Fact ]
    public void LoadAuthorizationConfiguration_WithConfiguration_LoadsConfigurationSuccessfully()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection( new Dictionary<string, string?>
                            {
                                { "Authorization:AccessTokenUrl", "TokenUrl" },
                                { "Authorization:GrantType", "ClientCredentials" },
                                { "Authorization:ClientId", "client_id" },
                                { "Authorization:ClientSecret", "client_secret" },
                                { "Authorization:Scope", "some_scope" }
                            } )
                            .Build();

        // Act
        var appConfig = configuration.GetSection( "Authorization" ).LoadAuthorizationConfiguration();

        // Assert
        appConfig.Should().NotBeNull();

        appConfig.Should().BeEquivalentTo(
            new AuthorizationConfiguration
            {
                AccessTokenUrl = "TokenUrl",
                GrantType = GrantType.ClientCredentials,
                ClientId = "client_id",
                ClientSecret = "client_secret",
                Scope = "some_scope"
            } );
    }

    [ Fact ]
    public void LoadAuthorizationConfiguration_WithoutConfiguration_GetsEmptyConfiguration()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .Build();

        // Act
        var act = () => configuration.LoadAuthorizationConfiguration();

        // Assert
        act.Should().Throw<ConfigurationException>()
           .WithMessage( "Configuration parameter 'AccessTokenUrl' not defined." );
    }
    #endregion
}