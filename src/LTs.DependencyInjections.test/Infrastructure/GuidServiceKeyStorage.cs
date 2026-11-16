using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace LTs.DependencyInjections.test.Infrastructure;

internal class GuidServiceKeyStorage : IStorage
{
    public GuidServiceKeyStorage(
        [ServiceKey]
        [TypeConverter( typeof( GuidStringTypeConverter ) )]
        Guid resolvedKey )
    {
        ResolvedKey = resolvedKey;
    }

    public Guid ResolvedKey { get; }

    public string Name => ResolvedKey.ToString();
}