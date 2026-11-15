using LTs.Json.Converters;
using Newtonsoft.Json;

namespace LTs.Json.test.Infrastructure;

internal class NameEnumConverterPayload
{
    [ JsonConverter( typeof( NameEnumConverter<SampleEnum> ) ) ]
    public required SampleEnum Status { get; init; }
}