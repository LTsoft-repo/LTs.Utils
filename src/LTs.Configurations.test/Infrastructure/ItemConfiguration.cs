using System.ComponentModel.DataAnnotations;

namespace LTs.Configurations.test.Infrastructure;

internal record ItemConfiguration
{
    [ Required ]
    public string Name { get; init; } = string.Empty;
}
