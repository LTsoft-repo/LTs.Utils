using JetBrains.Annotations;
using LTs.Logging;
using LTs.Logging.Configurations;
using LTs.Logging.Wrappers;
using Serilog;

namespace LTs.Logging.test.Mvc;

[ UsedImplicitly ]
public class Program
{
    public static bool UseTokenReplacement = true;

    public static void Main( string[] args )
    {
        ConfigureLog();

        var builder = WebApplication.CreateBuilder( args );
        builder.Host.UseSerilog();

        var app = builder.Build();
        app.UseSerilogRequestLogging();

        app.MapGet( "/", () => "Hello World!" );

        app.Run();
    }

    private static void ConfigureLog() => LogConfigurator.ConfigureWithTransform(
        new LogConfiguration(),
        UseTokenReplacement ? new[] { new ReplaceAccessTokenLogTransformation() } : Enumerable.Empty<ILogTransformation>(),
        loggerConfiguration =>
            {
                loggerConfiguration
                    .Enrich.WithClientIp()
                    .Enrich.WithRequestHeader( "User-Agent" );
            } );
}
