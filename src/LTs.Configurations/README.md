# LTs.Configurations

## Introduction
Set of classes and extensions for loading, validating, and registering configurations.

This library includes:
- LTs.Configurations.Abstractions
  - IAutofacConfigurationProvider
  - IConfigurationProvider
- LTs.Configurations.Configurations
  - ScheduleConfiguration
  - ScheduleConfiguration&lt;T&gt;
  - ScheduleConfigurationLoader.LoadScheduleConfiguration()
  - ScheduleConfigurationLoader.LoadScheduleConfiguration&lt;T&gt;()
  - ScheduleConfigurationRegistrationExtensions.AddScheduleConfiguration()
  - ScheduleConfigurationRegistrationExtensions.AddScheduleConfiguration&lt;T&gt;()
- LTs.Configurations.DependencyInjection
  - RegistrationExtensions.AddConfigurationProvider()
- LTs.Configurations.Exceptions
  - ConfigurationException
  - ConfigurationException.ThrowIfNull()
  - ConfigurationException.ThrowIfNullOrEmpty()
  - ConfigurationException.ThrowIfNullOrWhiteSpace()
- LTs.Configurations.Extensions
  - ConfigurationBuilderEmptyStringExtensions.ParseEmptyString()
  - GetConfigurationExtensions.GetRequiredValue&lt;T&gt;()
  - GetConfigurationExtensions.GetRequiredConnectionString()
  - GetConfigurationExtensions.GetSectionPath()
  - LoadConfigurationExtensions.AddDefaultConfigurationForAssembly()
- LTs.Configurations
  - AutofacConfigurationProvider
  - ConfigurationProvider
