using LTs.Json.test.Infrastructure;
using Newtonsoft.Json;

namespace LTs.Json.test.Converters;

public class NameEnumConverterTest : BaseTest
{
    public NameEnumConverterTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Serialize
    [ Fact ]
    public void Serialize_WritesMemberName()
    {
        // Arrange
        var payload = new NameEnumConverterPayload { Status = SampleEnum.Value2 };

        // Act
        var result = JsonConvert.SerializeObject( payload );

        // Assert
        result.Should().Be( """{"Status":"Value2"}""" );
    }
    #endregion

    #region Deserialize
    [ Fact ]
    public void Deserialize_ReadsMemberName()
    {
        // Arrange
        var json = """{"Status":"Value2"}""";

        // Act
        var result = JsonConvert.DeserializeObject<NameEnumConverterPayload>( json );

        // Assert
        result.Should().BeEquivalentTo( new NameEnumConverterPayload { Status = SampleEnum.Value2 } );
    }
    #endregion
}