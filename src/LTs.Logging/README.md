# LTs.Logging

## Introduction
Set of classes and extensions for logging using Serilog.

This library includes:
- LTs.Logging.Configurations
  - LogConfiguration
  - LogConfigurationDefaults
  - LogConfigurationExtensions.LoadLogConfiguration()
- LTs.Logging.DependencyInjection
  - AutofacLoggerProvider
  - RegistrationExtensions.AddSerilog()
  - RegistrationExtensions.AddLogConfiguration()
  - RegistrationExtensions.RegisterLogConfiguration() (obsolete; use AddLogConfiguration)
- LTs.Logging.Serilog.Abstractions
  - IFlushableSink
- LTs.Logging.Serilog.Sinks
  - PeriodicFlushSink&lt;TSink&gt;
- LTs.Logging.Wrappers
  - LogTransformationFunc
  - TransformConditionFunc
  - ILogTransformation
  - LoggerSinkConfigurationExtensions.TransformLog()
  - LogTransformation
  - ReplaceAccessTokenLogTransformation
  - ReplaceTextLogTransformation
  - TransformLogSinkWrapper
- LogConfigurator.FileWithConfiguration()
- LogConfigurator.Configure()
- LogConfigurator.ConfigureWithTransform()
- LogConfigurator.ConfigureWithIp()
