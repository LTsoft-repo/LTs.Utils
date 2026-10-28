using LTs.Json.Extensions;

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
        var options = new JsonStringOptions { UseIndent = true };

        // Act
        var result = token.ToJson( options );

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

        var options = new JsonStringOptions { UseIndent = true, Minify = true };

        // Act
        var result = token.ToJson( options );

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
        var options = new JsonStringOptions { UseIndent = true };

        // Act
        var result = value.ToJson( options );

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
        var options = new JsonStringOptions { UseIndent = true, Minify = true };

        // Act
        var result = value.ToJson( options );

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
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
    #endregion
}
