using LTs.Json.Extensions;

namespace LTs.Json.test.Extensions;

public class JTokenSerializeExtensionsTest : BaseTest
{
    public JTokenSerializeExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void ToJson_DefaultOptions_ReturnsMinifiedJson()
    {
        // Arrange
        var token = """{ "name": "John", "age": 30 }""".ParseAsJToken();

        // Act
        var result = token.ToJson();

        // Assert
        result.Should().Be( """{"name":"John","age":30}""" );
    }

    [ Fact ]
    public void ToJson_OptionIndent_ReturnsIndentedJson()
    {
        // Arrange
        var token = """{ "name": "John", "age": 30 }""".ParseAsJToken();
        var options = new JsonStringOptions { UseIndent = true };

        // Act
        var result = token.ToJson( options );

        // Assert
        result.Should().Contain( Environment.NewLine )
              .And.Contain( "  \"name\": \"John\"" );
    }

    [ Fact ]
    public void ToJson_OptionMinify_ReturnsMinifiedJson()
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
}