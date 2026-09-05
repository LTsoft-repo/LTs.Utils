using Autofac;
using LTs.Configurations.Abstractions;
using LTs.DependencyInjections.DependencyInjection;
using LTs.DependencyInjections.Extensions;
using LTs.Hosting.Extensions;
using LTs.Hosting.test.Infrastructure;
using LTs.Logging.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Serilog.Extensions.Logging;
using IConfigurationProvider = LTs.Configurations.Abstractions.IConfigurationProvider;

namespace LTs.Hosting.test.Extensions;

public class HostApplicationBuilderExtensionsTest : BaseTest
{
    public HostApplicationBuilderExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ConfigureDefaultHostApplicationT
    [ Fact ]
    public void ConfigureDefaultHostApplicationT_WithValidParameters_Successes()
    {
        // Arrange
        var hostBuilder = new HostApplicationBuilder();
        IConfigurationProvider? configurationProvider = null;

        Environment.SetEnvironmentVariable( "Key", "Value" );

        // Act
        hostBuilder.ConfigureDefaultHostApplication<ClassInAssembly>(
            "ServiceName",
            configContainerBuilder => configContainerBuilder.Register( _ => new LogConfiguration() ).SingleInstance(),
            ( builder, configProvider ) =>
                {
                    builder.RegisterType<SomeServiceClass>();
                    configurationProvider = configProvider;
                },
            serviceBuilder => serviceBuilder.AddHostedService<TestWorkerClass>(),
            ( _, _ ) => { } );

        var host = hostBuilder.Build();
        Disposables.Add( host );

        // Assert
        host.Should().NotBeNull();

        configurationProvider.Should().NotBeNull();
        var providerConfiguration = configurationProvider!.Get<IConfiguration>();
        var configurationKey = providerConfiguration[ "Key" ];
        configurationKey.Should().Be( "Value" );

        var logConfiguration = configurationProvider.Get<LogConfiguration>();
        logConfiguration.Should().NotBeNull();

        var configuration = host.Services.GetRequiredService<IConfiguration>();
        configuration.Should().NotBeNull();
        var hostConfigurationKey = configuration[ "Key" ];
        hostConfigurationKey.Should().Be( "Value" );

        var configurationClass = host.Services.GetRequiredService<LogConfiguration>();
        configurationClass.Should().BeOfType<LogConfiguration>();

        var someService = host.Services.GetRequiredService<SomeServiceClass>();
        someService.Should().BeOfType<SomeServiceClass>();

        var anotherService = host.Services.GetRequiredService<IHostedService>();
        anotherService.Should().BeOfType<TestWorkerClass>();

        Environment.SetEnvironmentVariable( "Key", null );
    }
    #endregion

    #region ConfigureDefaultHostApplication
    [ Fact ]
    public void ConfigureDefaultHostApplication_AutofacDITest_Successes()
    {
        // Arrange
        var hostBuilder = new HostApplicationBuilder();

        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterType<SomeConfigurationClass>();
        containerBuilder.RegisterType<SomeServiceClass>();
        var extraContainer = containerBuilder.Build();

        // Act
        hostBuilder.ConfigureContainer( new AutofacServiceProviderFactory(),
                                        b =>
                                            {
                                                b.Populate( extraContainer );
                                            } );

        var host = hostBuilder.Build();
        Disposables.Add( host );

        // Assert
        host.Should().NotBeNull();

        var configurationClass = host.Services.GetRequiredService<SomeConfigurationClass>();
        configurationClass.Should().NotBeNull();

        var someService = host.Services.GetRequiredService<SomeServiceClass>();
        someService.Should().NotBeNull();
    }

    [ Fact ]
    public void ConfigureDefaultHostApplication_MockedParameters_Successes()
    {
        // Arrange
        var hostBuilder = new HostApplicationBuilder();

        var loadConfigurationAction = new Mock<Action<IConfigurationBuilder>>();
        loadConfigurationAction.Setup( x => x( It.IsAny<IConfigurationBuilder>() ) );

        var registerConfigurationAction = new Mock<Action<ContainerBuilder>>();

        registerConfigurationAction.Setup( x => x( It.IsAny<ContainerBuilder>() ) )
                                   .Callback<ContainerBuilder>( builder =>
                                       {
                                           builder.RegisterInstance( new LogConfiguration() );
                                       } );

        var registerServicesAction = new Mock<Action<ContainerBuilder, IAutofacConfigurationProvider>>();

        registerServicesAction.Setup( x => x( It.IsAny<ContainerBuilder>(), It.IsAny<IAutofacConfigurationProvider>() ) );

        var registerServicesCollectionAction = new Mock<Action<IServiceCollection>>();
        registerServicesCollectionAction.Setup( x => x( It.IsAny<IServiceCollection>() ) );

        var additionalLoggingConfigurationAction = new Mock<Action<LoggerConfiguration, IAutofacConfigurationProvider>>();
        additionalLoggingConfigurationAction.Setup( x => x( It.IsAny<LoggerConfiguration>(), It.IsAny<IAutofacConfigurationProvider>() ) );

        // Act
        hostBuilder.ConfigureDefaultHostApplication(
            "ServiceName",
            loadConfigurationAction.Object,
            registerConfigurationAction.Object,
            registerServicesAction.Object,
            registerServicesCollectionAction.Object,
            additionalLoggingConfigurationAction.Object
        );

        var host = hostBuilder.Build();
        Disposables.Add( host );

        // Assert
        loadConfigurationAction.Verify( x => x( It.IsAny<IConfigurationBuilder>() ), Times.Once );
        registerConfigurationAction.Verify( x => x( It.IsAny<ContainerBuilder>() ), Times.Once );
        registerServicesCollectionAction.Verify( x => x( It.IsAny<IServiceCollection>() ), Times.Once );

        additionalLoggingConfigurationAction.Verify(
            x => x( It.IsAny<LoggerConfiguration>(), It.IsAny<IAutofacConfigurationProvider>() ),
            Times.Once );

        host.Should().NotBeNull();
        registerServicesAction.Verify( x => x( It.IsAny<ContainerBuilder>(), It.IsAny<IAutofacConfigurationProvider>() ), Times.Once );
    }

    [ Fact ]
    public void ConfigureDefaultHostApplication_WithValidParameters_Successes()
    {
        // Arrange
        var hostBuilder = new HostApplicationBuilder();
        IConfigurationProvider? configurationProvider = null;

        // Act
        hostBuilder.ConfigureDefaultHostApplication(
            "ServiceName",
            configBuilder => configBuilder.AddInMemoryCollection( new Dictionary<string, string?> { { "Key", "Value" } } ),
            configContainerBuilder => configContainerBuilder.Register( _ => new LogConfiguration() ).SingleInstance(),
            ( builder, configProvider ) =>
                {
                    builder.RegisterType<SomeServiceClass>();
                    configurationProvider = configProvider;
                },
            serviceBuilder => serviceBuilder.AddHostedService<TestWorkerClass>(),
            ( loggerConfig, configProvider ) =>
                {
                    loggerConfig.Should().NotBeNull();
                    configProvider.Should().NotBeNull();
                }
        );

        var host = hostBuilder.Build();
        Disposables.Add( host );

        // Assert
        host.Should().NotBeNull();

        configurationProvider.Should().NotBeNull();
        var providerConfiguration = configurationProvider!.Get<IConfiguration>();
        var configurationKey = providerConfiguration[ "Key" ];
        configurationKey.Should().Be( "Value" );

        var logConfiguration = configurationProvider.Get<LogConfiguration>();
        logConfiguration.Should().NotBeNull();

        var configuration = host.Services.GetRequiredService<IConfiguration>();
        configuration.Should().NotBeNull();
        var hostConfigurationKey = configuration[ "Key" ];
        hostConfigurationKey.Should().Be( "Value" );

        var loggerProvider = host.Services.GetService<ILoggerProvider>();
        loggerProvider.Should().BeOfType<SerilogLoggerProvider>();

        var configurationClass = host.Services.GetRequiredService<LogConfiguration>();
        configurationClass.Should().BeOfType<LogConfiguration>();

        var someService = host.Services.GetRequiredService<SomeServiceClass>();
        someService.Should().BeOfType<SomeServiceClass>();

        var anotherService = host.Services.GetRequiredService<IHostedService>();
        anotherService.Should().BeOfType<TestWorkerClass>();
    }
    #endregion
}
