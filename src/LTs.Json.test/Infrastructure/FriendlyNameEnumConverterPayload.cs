using LTs.Json.Converters;
using Newtonsoft.Json;

namespace LTs.Json.test.Infrastructure;

internal class FriendlyNameEnumConverterPayload
{
    [ JsonConverter( typeof( FriendlyNameEnumConverter<SampleEnum> ) ) ]
    public required SampleEnum Status { get; init; }
}