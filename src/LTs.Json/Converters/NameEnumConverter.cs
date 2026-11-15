using LTs.Utils.Extensions.Enums;
using Newtonsoft.Json;

namespace LTs.Json.Converters;

/// <summary>
///     A JSON converter that serializes and deserializes enum values using their value names.
/// </summary>
/// <typeparam name="TEnum">Type of the enum.</typeparam>
public class NameEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    /// <inheritdoc />
    public override void WriteJson( JsonWriter writer,
                                    TEnum value,
                                    JsonSerializer serializer )
        => writer.WriteValue( value.ToValueString() );

    /// <inheritdoc />
    public override TEnum ReadJson( JsonReader reader,
                                    Type objectType,
                                    TEnum existingValue,
                                    bool hasExistingValue,
                                    JsonSerializer serializer )
    {
        if( reader.TokenType != JsonToken.String
            || reader.Value is not string text )
        {
            throw new JsonSerializationException( "Expected a string." );
        }

        return text.ToEnum<TEnum>();
    }
}