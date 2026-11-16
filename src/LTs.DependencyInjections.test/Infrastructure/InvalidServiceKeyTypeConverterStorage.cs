using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class InvalidServiceKeyTypeConverterStorage : IStorage
{
    public InvalidServiceKeyTypeConverterStorage(
        [ServiceKey]
        [TypeConverter( typeof( SomeClass ) )]
        Guid resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public Guid ResolvedKey { get; }

    public string Name => ResolvedKey.ToString();
}