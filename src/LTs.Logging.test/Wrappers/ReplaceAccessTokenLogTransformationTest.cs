using LTs.Logging;
using LTs.Logging.Wrappers;
using LTs.Logging.test.Infrastructure;
using LTs.Logging.test.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Serilog;

namespace LTs.Logging.test.Wrappers;

[ Collection( "Sequential" ) ]
public class ReplaceAccessTokenLogTransformationTest : BaseTest
{
    private readonly TextWriter oldWriter;

    protected readonly StringWriter StringWriter;

    public ReplaceAccessTokenLogTransformationTest( ITestOutputHelper testOutput )
        : base( testOutput )
    {
        oldWriter = Console.Out;
        StringWriter = new();
        Console.SetOut( StringWriter );
    }

    public override void Dispose()
    {
        Console.SetOut( oldWriter );
        StringWriter.Dispose();
        base.Dispose();
    }

    #region ReplaceAccessTokenLogTransformation
    [ Fact ]
    public void ReplaceAccessTokenLogTransformation_WithWrapper_ReplacesToken()
    {
        // Arrange
        LogConfigurator.ConfigureWithTransform(
            new(),
            new[] { new ReplaceAccessTokenLogTransformation() },
            loggerConfiguration =>
                {
                    loggerConfiguration
                        .Enrich.WithClientIp()
                        .Enrich.WithRequestHeader( "User-Agent" );
                } );

        // Act
        Log.Logger.Information( "Request starting HTTP/1.1 GET https://localhost:5191/version?access_token=A1B2C3d4E5FG678" );
        Log.Logger.Information( "Request finished HTTP/1.1 GET https://localhost:5191/version?access_token=A1B2C3D4E5FG678" );
        var logs = StringWriter.ToString();

        // Assert
        logs.Should().MatchRegex( @"\[INF\] Request starting HTTP\/.*? GET .*?access_token=<NotShown> .*" );
        logs.Should().MatchRegex( @"\[INF\] Request finished HTTP\/.*? GET .*?access_token=<NotShown> .*" );

        logs.Should().NotContain( "A1B2C3d4E5FG678" );
        logs.Should().NotContain( "A1B2C3D4E5FG678" );
    }

    [ Fact ]
    public void ReplaceAccessTokenLogTransformation_WithoutWrapper_DoNotReplaceToken()
    {
        // Arrange
        LogConfigurator.ConfigureWithTransform(
            new(),
            Enumerable.Empty<ILogTransformation>(),
            loggerConfiguration =>
                {
                    loggerConfiguration
                        .Enrich.WithClientIp()
                        .Enrich.WithRequestHeader( "User-Agent" );
                } );

        // Act
        Log.Logger.Information( "Request starting HTTP/1.1 GET https://localhost:5191/version?access_token=A1B2C3d4E5FG678" );
        Log.Logger.Information( "Request finished HTTP/1.1 GET https://localhost:5191/version?access_token=A1B2C3D4E5FG678" );
        var logs = StringWriter.ToString();

        // Assert
        logs.Should().MatchRegex( @"\[INF\] Request starting HTTP\/.*? GET .*?access_token=A1B2C3d4E5FG678 .*" );
        logs.Should().MatchRegex( @"\[INF\] Request finished HTTP\/.*? GET .*?access_token=A1B2C3D4E5FG678 .*" );
    }

    [ Fact ]
    public async Task ReplaceAccessTokenLogTransformation_FromMvcWithReplacement_ReplaceToken()
    {
        // Arrange
        Program.UseTokenReplacement = true;

        var server = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(
                builder =>
                    {
                        builder.ConfigureServices(
                            _ =>
                                {
                                } );
                    } );

        var client = server.CreateClient();

        // Act
        _ = await client.GetStringAsync( "/?access_token=A1B2C3d4E5FG678" );
        await Task.Delay( 200 );
        var logs = StringWriter.ToString();

        // Assert
        logs.Should().MatchRegex( @"\[INF\] Request starting HTTP\/.*? GET .*?access_token=<NotShown> .*" );
        logs.Should().MatchRegex( @"\[INF\] Request finished HTTP\/.*? GET .*?access_token=<NotShown> .*" );

        logs.Should().NotContain( "A1B2C3d4E5FG678" );
        logs.Should().NotContain( "A1B2C3D4E5FG678" );
    }

    [ Fact ]
    public async Task ReplaceAccessTokenLogTransformation_FromMvcWithoutReplacement_DoNotReplaceToken()
    {
        // Arrange
        Program.UseTokenReplacement = false;

        var server = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(
                builder =>
                    {
                        builder.ConfigureServices(
                            _ =>
                                {
                                } );
                    } );

        var client = server.CreateClient();

        // Act
        _ = await client.GetStringAsync( "/?access_token=A1B2C3d4E5FG678" );
        await Task.Delay( 200 );
        var logs = StringWriter.ToString();

        // Assert
        logs.Should().MatchRegex( @"\[INF\] Request starting HTTP\/.*? GET .*?access_token=A1B2C3d4E5FG678 .*" );
        logs.Should().MatchRegex( @"\[INF\] Request finished HTTP\/.*? GET .*?access_token=A1B2C3d4E5FG678 .*" );
    }
    #endregion
}
