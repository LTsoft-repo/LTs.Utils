using LTs.Json.Extensions;
using Newtonsoft.Json.Linq;

namespace LTs.Json.test.Extensions;

public class StringDeserializeExtensionsTest : BaseTest
{
    public StringDeserializeExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    [ Fact ]
    public void ParseAsJToken_DefaultOptions_ParsesDateAsString()
    {
        // Arrange
        var json = """{ "date": "2026-07-05T01:02:03Z" }""";

        // Act
        var result = json.ParseAsJToken();

        // Assert
        result[ "date" ]!.Type.Should().Be( JTokenType.String );
        result[ "date" ]!.Value<string>().Should().Be( "2026-07-05T01:02:03Z" );
    }

    [ Fact ]
    public void ParseAsJToken_ParseAsDateTime_ParsesDateAsDateTime()
    {
        // Arrange
        var json = """{ "date": "2026-07-05T01:02:03Z" }""";
        var options = new JsonParseOptions { DateParseType = JsonDateParseType.DateTime };

        // Act
        var result = json.ParseAsJToken( options );

        // Assert
        result[ "date" ]!.Type.Should().Be( JTokenType.Date );
        ( (JValue)result[ "date" ]! ).Value.Should().BeOfType<DateTime>();
    }

    [ Fact ]
    public void ParseAsJToken_ParseAsDateTimeOffset_ParsesDateAsDateTimeOffset()
    {
        // Arrange
        var json = """{ "date": "2026-07-05T01:02:03Z" }""";
        var options = new JsonParseOptions { DateParseType = JsonDateParseType.DateTimeOffset };

        // Act
        var result = json.ParseAsJToken( options );

        // Assert
        result[ "date" ]!.Type.Should().Be( JTokenType.Date );
        ( (JValue)result[ "date" ]! ).Value.Should().BeOfType<DateTimeOffset>();
    }
}