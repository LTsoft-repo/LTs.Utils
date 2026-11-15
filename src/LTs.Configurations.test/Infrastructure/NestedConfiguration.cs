using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record NestedConfiguration
{
    [ Required ]
    public required string RequiredValue { get; init; }

    public int OptionalCount { get; init; } = 3;
}