using System.ComponentModel.DataAnnotations;

namespace LTs.Configurations.test.Infrastructure;

internal record SampleConfiguration
{
    [ Required ]
    public string RequiredName { get; init; } = string.Empty;

    public string OptionalDescription { get; init; } = "default-description";

    public int OptionalCount { get; init; } = 5;

    public Uri OptionalUri { get; init; } = new( "http://localhost/" );

    public TimeSpan OptionalTimeout { get; init; } = TimeSpan.FromMinutes( 1 );
}