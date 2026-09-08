using LTs.Configurations.Attributes;

namespace LTs.Configurations.test.Infrastructure;

internal record RequiredStringConfiguration
{
    [ Required ]
    public string RequiredValue { get; init; } = string.Empty;
}
