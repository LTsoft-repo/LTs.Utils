# LTs.Web

## Introduction
Set of classes to be used as Http Client, helping to be able to mock it in unit tests.

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
  - HttpHandlerRegistrationExtensions.AddHttpHandler()