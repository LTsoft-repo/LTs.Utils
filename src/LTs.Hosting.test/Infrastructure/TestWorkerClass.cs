using Microsoft.Extensions.Hosting;

namespace LTs.Hosting.test.Infrastructure;

internal class TestWorkerClass : IHostedService
{
    public Task StartAsync( CancellationToken cancellationToken ) => Task.CompletedTask;

    public Task StopAsync( CancellationToken cancellationToken ) => Task.CompletedTask;
}
