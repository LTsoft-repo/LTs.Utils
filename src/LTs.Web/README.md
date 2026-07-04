# LTs.Web

## Introduction
Set of HTTP client abstractions, configuration models, MIME helpers, and web extensions.

This library includes:
- LTs.Web.Abstractions
  - IHttpHandler
- LTs.Web.Authorization
  - AuthorizationData
  - GrantType
  - GrantTypeExtensions.ToIdentityString()
  - GrantTypeExtensions.ToGrantType()
  - GrantTypes.StringRepresentation
- LTs.Web.Configurations
  - AuthorizationConfiguration
  - AuthorizationConfigurationLoader.LoadAuthorizationConfiguration()
- LTs.Web.DependencyInjection
  - RegistrationExtensions.AddHttpHandler()
- LTs.Web.Extensions
  - HttpContentExtensions.ReadAsJsonAsync()
  - HttpContentExtensions.ReadAsJsonAsync&lt;T&gt;()
  - HttpContentExtensions.ReadFormAsync()
  - StringWebExtensions.AddQueryString()
  - StringWebExtensions.CombineUri()
  - StringWebExtensions.AddQueryStringIfValueNotNull()
- LTs.Web.Mime
  - MediaType
  - MediaTypeNames
  - MediaTypeExtensions.ToMediaTypeString()
  - MediaTypeExtensions.ToMediaType()
  - MediaTypeNames.None
  - MediaTypeNames.Text
  - MediaTypeNames.Application
  - MediaTypeNames.Image
  - MediaTypeNames.Multiparts
- LTs.Web
  - HttpHandler
