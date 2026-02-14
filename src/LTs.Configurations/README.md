# LTs.Configurations

## Introduction
Set of classes and extensions for configurations.

This library includes:
- LTs.Configurations.Abstractions
  - IAutofacConfigurationProvider
  - IConfigurationProvider
- LTs.Configurations.Configurations
  - ScheduleConfiguration
  - ScheduleConfigurationLoader.LoadScheduleConfiguration()
  - ScheduleConfigurationLoader.LoadScheduleConfiguration&lt;T&gt;()
  - ScheduleConfigurationRegistrationExtensions.AddScheduleConfiguration()
  - ScheduleConfigurationRegistrationExtensions.AddScheduleConfiguration&lt;T&gt;()
- LTs.Configurations.Exceptions
  - ConfigurationException
- LTs.Configurations.Extensions
  - ConfigurationBuilderEmptyStringExtensions.ParseEmptyString()
  - ConfigurationProviderRegistrationExtensions.AddConfigurationProvider()
  - GetConfigurationExtensions.GetRequiredValue&lt;T&gt;()
  - GetConfigurationExtensions.GetRequiredConnectionString()
  - GetConfigurationExtensions.GetSectionPath()
  - LoadConfigurationExtensions.AddDefaultConfigurationForAssembly()
- LTs.Configurations
  - AutofacConfigurationProvider
  - ConfigurationProvider