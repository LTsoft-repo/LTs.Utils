using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record RequiredStringConfiguration
{
    [ Required ]
    public required string RequiredValue { get; init; }
}