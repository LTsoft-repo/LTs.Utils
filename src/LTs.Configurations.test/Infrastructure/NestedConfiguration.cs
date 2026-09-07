using System.ComponentModel.DataAnnotations;

namespace LTs.Configurations.test.Infrastructure;

internal record NestedConfiguration
{
    [ Required ]
    public string RequiredValue { get; init; } = string.Empty;

    public int OptionalCount { get; init; } = 3;
}
