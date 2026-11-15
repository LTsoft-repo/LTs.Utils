using System.Globalization;
using LTs.Json.Extensions;
using LTs.Utils.Extensions.Enums;
using Newtonsoft.Json;

// ReSharper disable ConvertToPrimaryConstructor

namespace LTs.Json.Converters;

// Handles every enum on serializers built from JsonStringOptions; property JsonConverter attributes still win.
// Write uses EnumsAsFriendlyNames / EnumsAsStrings when set; otherwise writes the underlying integer.
// Read accepts string (ToEnum) or integer so JSON can use either shape regardless of write flags.
#pragma warning disable IDE0290
internal sealed class EnumJsonConverter : JsonConverter
{
    private readonly JsonStringOptions options;

    public EnumJsonConverter( JsonStringOptions options )
    {
        this.options = options;
    }

    public override bool CanConvert( Type objectType )
    {
        var type = Nullable.GetUnderlyingType( objectType ) ?? objectType;

        return type.IsEnum;
    }

    public override void WriteJson( JsonWriter writer, object? value, JsonSerializer serializer )
    {
        if( value is null )
        {
            writer.WriteNull();

            return;
        }

        var enumValue = (Enum)value;

        if( options.HasFlag( JsonStringOptions.EnumsAsFriendlyNames ) )
        {
            writer.WriteValue( enumValue.ToFriendlyName() );

            return;
        }

        if( options.HasFlag( JsonStringOptions.EnumsAsStrings ) )
        {
            writer.WriteValue( enumValue.ToValueString() );

            return;
        }

        var underlyingType = Enum.GetUnderlyingType( enumValue.GetType() );
        var underlyingValue = Convert.ChangeType( enumValue, underlyingType, CultureInfo.InvariantCulture );

        writer.WriteValue( underlyingValue );
    }

    public override object? ReadJson( JsonReader reader,
                                      Type objectType,
                                      object? existingValue,
                                      JsonSerializer serializer )
    {
        var enumType = Nullable.GetUnderlyingType( objectType ) ?? objectType;
        var isNullable = Nullable.GetUnderlyingType( objectType ) is not null;

        if( reader.TokenType == JsonToken.Null )
        {
            if( isNullable )
            {
                return null;
            }

            throw new JsonSerializationException( "Cannot convert null to non-nullable enum." );
        }

        if( reader.TokenType == JsonToken.String
            && reader.Value is string text )
        {
            return Enum.ToObject( enumType, text.ToEnum( enumType ) );
        }

        if( reader.TokenType == JsonToken.Integer )
        {
            var numeric = Convert.ToInt64( reader.Value, CultureInfo.InvariantCulture );

            return Enum.ToObject( enumType, numeric );
        }

        throw new JsonSerializationException( $"Unexpected token {reader.TokenType} when parsing enum." );
    }
}