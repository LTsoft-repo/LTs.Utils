# LTs.TestUtils

## Introduction
Set of classes, assertions, loggers, and web helpers for tests.

This library includes:
- LTs.TestUtils.Configurations
  - ConfigurationManager
- LTs.TestUtils.FluentAssertions
  - GenericCollectionAssertionsExtensions.ContainEquivalentSubset()
  - GenericCollectionAssertionsExtensions.NotContainEquivalentInSubset()
  - GenericCollectionAssertionsExtensions.ContainExactlyEquivalent()
  - HttpResponseMessageFluentAssertionExtensions.HaveContentAsJsonAsync()
  - HttpResponseMessageFluentAssertionExtensions.HaveContentWithMediaType()
  - HttpResponseMessageFluentAssertionExtensions.ContainsHeaderWithValues()
  - HttpResponseMessageFluentAssertionExtensions.NotContainsHeader()
  - JsonAssertionsExtensions.BeSameJsonAs()
  - JsonAssertionsExtensions.NotBeSameJsonAs()
  - JsonAssertionsExtensions.BeSameJsonIgnoringExtraFieldsAs()
- LTs.TestUtils.Loggers.DependencyInjection
  - RegistrationExtensions.AddTestLogger()
- LTs.TestUtils.Loggers
  - InMemoryLogger
  - InMemoryLogger&lt;T&gt;
  - LoggerMessage
  - TestLogger
  - TestLogger&lt;T&gt;
  - TestLoggerProvider
- LTs.TestUtils.Tests
  - BaseTest
- LTs.TestUtils.Web
  - MockHttpClientExtensions.ShouldBeEquivalentTo()
  - MockHttpClientFactory.CreateForGet()
  - MockHttpClientFactory.CreateToEchoRequest()
  - MockHttpClientFactory.CreateForIdentity()
  - MockHttpHandlerFactory.CreateToEchoRequest()
  - MockHttpHandlerFactoryEchoResponseContent
- LTs.TestUtils
  - DiagnosticMessage
  - TestUtility.GetTestConfiguration()
  - TestUtility.AddTestSettings()
  - TestUtility.RetryAsync()
  - Wait.ForAsync()
