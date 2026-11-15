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
        var result = token.ToJson( JsonStringOptions.Minify );

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
    #endregion
}
