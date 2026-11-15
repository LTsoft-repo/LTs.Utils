using LTs.Json.Extensions;
using LTs.Json.test.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LTs.Json.test.Extensions;

public class JTokenSerializeExtensionsTest : BaseTest
{
    public JTokenSerializeExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ToJson (JToken)
    [ Fact ]
    public void ToJson_JToken_ReturnsMinifiedJson()
    {
        // Arrange
        var token = """{ "name": "John", "age": 30 }""".ParseAsJToken();

        // Act
        var result = token.ToJson();

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_JToken_WhenUseIndent_ReturnsIndentedJson()
    {
        // Arrange
        var token = """{ "name": "John", "age": 30 }""".ParseAsJToken();

        // Act
        var result = token.ToJson( JsonStringOptions.UseIndent );

        // Assert
        result.Should().Be(
            """
            {
              "name": "John",
              "age": 30
            }
            """ );
    }

    [ Fact ]
    public void ToJson_JToken_WhenMinify_ReturnsMinifiedJson()
    {
        // Arrange
        var token = """
                    {
                      "name": "John",
                      "age": 30
                    }
                    """.ParseAsJToken();

        // Act
        var result = token.ToJson();

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_JToken_WhenMinifyAndUseIndent_PrefersMinify()
    {
        // Arrange
        var token = """{ "name": "John", "age": 30 }""".ParseAsJToken();

        // Act
        var result = token.ToJson( JsonStringOptions.Minify | JsonStringOptions.UseIndent );

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }
    #endregion

    #region ToJson (Object)
    [ Fact ]
    public void ToJson_Object_ReturnsMinifiedJson()
    {
        // Arrange
        var value = new { name = "John", age = 30 };

        // Act
        var result = value.ToJson();

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenUseIndent_ReturnsIndentedJson()
    {
        // Arrange
        var value = new { name = "John", age = 30 };

        // Act
        var result = value.ToJson( JsonStringOptions.UseIndent );

        // Assert
        result.Should().Be(
            """
            {
              "name": "John",
              "age": 30
            }
            """ );
    }

    [ Fact ]
    public void ToJson_Object_WhenMinify_ReturnsMinifiedJson()
    {
        // Arrange
        var value = new { name = "John", age = 30 };

        // Act
        // ReSharper disable once RedundantArgumentDefaultValue
        var result = value.ToJson( JsonStringOptions.Minify );

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenIgnoreNulls_OmitsNullProperties()
    {
        // Arrange
        var value = new SamplePayload { Name = "John", Age = 30 };

        // Act
        var result = value.ToJson( JsonStringOptions.Minify | JsonStringOptions.IgnoreNulls );

        // Assert
        result.Should().Be( """{"Name":"John","Age":30}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenUseIndentAndIgnoreNulls_ReturnsIndentedJsonWithoutNulls()
    {
        // Arrange
        var value = new SamplePayload { Name = "John", Age = 30 };

        // Act
        var result = value.ToJson( JsonStringOptions.UseIndent | JsonStringOptions.IgnoreNulls );

        // Assert
        result.Should().Be(
            """
            {
              "Name": "John",
              "Age": 30
            }
            """ );
    }

    [ Fact ]
    public void ToJson_Object_WhenNull_ReturnsNullLiteral()
    {
        // Arrange
        object? value = null;

        // Act
        var result = value.ToJson();

        // Assert
        result.Should().Be( "null" );
    }

    [ Fact ]
    public void ToJson_Object_WhenFormattingIndented_ReturnsIndentedJson()
    {
        // Arrange
        var value = new { name = "John", age = 30 };

        // Act
        var result = value.ToJson( Formatting.Indented );

        // Assert
        result.Should().Be(
            """
            {
              "name": "John",
              "age": 30
            }
            """ );
    }

    [ Fact ]
    public void ToJson_Object_WhenSerializerSettings_AppliesSettings()
    {
        // Arrange
        var value = new SamplePayload { Name = "John", Age = 30 };

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        // Act
        var result = value.ToJson( settings );

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenNoEnumOptions_SerializesEnumAsNumber()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value2 };

        // Act
        var result = value.ToJson();

        // Assert
        result.Should().Be( """{"Status":2}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenEnumsAsStrings_SerializesEnumAsMemberName()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value2 };

        // Act
        var result = value.ToJson( JsonStringOptions.Minify | JsonStringOptions.EnumsAsStrings );

        // Assert
        result.Should().Be( """{"Status":"Value2"}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenEnumsAsFriendlyNames_SerializesEnumAsFriendlyName()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value2 };

        // Act
        var result = value.ToJson( JsonStringOptions.Minify | JsonStringOptions.EnumsAsFriendlyNames );

        // Assert
        result.Should().Be( """{"Status":"Value 2"}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenEnumsAsStringsAndFriendlyNames_SerializesEnumAsFriendlyName()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value2 };

        // Act
        var result = value.ToJson( JsonStringOptions.EnumsAsStrings | JsonStringOptions.EnumsAsFriendlyNames );

        // Assert
        result.Should().Be( """{"Status":"Value 2"}""" );
    }

    [ Fact ]
    public void ToJson_Object_WhenDefaultOptions_RoundTripsNumericEnum()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value1 };
        var options = JsonStringOptions.Minify;
        var json = value.ToJson( options );

        // Act
        var result = JsonConvert.DeserializeObject<SampleEnumPayload>( json, options.ToSerializerSettings() );

        // Assert
        result.Should().BeEquivalentTo( value );
    }

    [ Fact ]
    public void ToJson_Object_WhenEnumsAsFriendlyNames_RoundTripsFriendlyName()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value1 };
        var options = JsonStringOptions.EnumsAsFriendlyNames;
        var json = value.ToJson( options );

        // Act
        var result = JsonConvert.DeserializeObject<SampleEnumPayload>( json, options.ToSerializerSettings() );

        // Assert
        result.Should().BeEquivalentTo( value );
    }

    [ Fact ]
    public void ToJson_Object_WhenEnumsAsStrings_RoundTripsMemberName()
    {
        // Arrange
        var value = new SampleEnumPayload { Status = SampleEnum.Value3 };
        var options = JsonStringOptions.EnumsAsStrings;
        var json = value.ToJson( options );

        // Act
        var result = JsonConvert.DeserializeObject<SampleEnumPayload>( json, options.ToSerializerSettings() );

        // Assert
        result.Should().BeEquivalentTo( value );
    }

    [ Fact ]
    public void ToJson_Object_WhenDefaultOptions_DeserializesStringEnumFromJson()
    {
        // Arrange
        var options = JsonStringOptions.Minify;
        var json = """{"Status":"Value 2"}""";

        // Act
        var result = JsonConvert.DeserializeObject<SampleEnumPayload>( json, options.ToSerializerSettings() );

        // Assert
        result.Should().BeEquivalentTo( new SampleEnumPayload { Status = SampleEnum.Value2 } );
    }

    [ Fact ]
    public void ToJson_Object_WhenDefaultOptions_DeserializesMemberNameStringEnumFromJson()
    {
        // Arrange
        var options = JsonStringOptions.Minify;
        var json = """{"Status":"Value2"}""";

        // Act
        var result = JsonConvert.DeserializeObject<SampleEnumPayload>( json, options.ToSerializerSettings() );

        // Assert
        result.Should().BeEquivalentTo( new SampleEnumPayload { Status = SampleEnum.Value2 } );
    }
    #endregion
}