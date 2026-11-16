using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class FromKeyedServicesConsumer
{
    public FromKeyedServicesConsumer( [FromKeyedServices( "blob" )] IStorage storage )
    {
        Storage = storage;
    }

    public IStorage Storage { get; }
}