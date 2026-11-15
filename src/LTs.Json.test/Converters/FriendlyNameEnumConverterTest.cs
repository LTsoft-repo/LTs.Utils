using LTs.Json.test.Infrastructure;
using Newtonsoft.Json;

namespace LTs.Json.test.Converters;

public class FriendlyNameEnumConverterTest : BaseTest
{
    public FriendlyNameEnumConverterTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region Serialize
    [ Fact ]
    public void Serialize_WritesFriendlyName()
    {
        // Arrange
        var payload = new FriendlyNameEnumConverterPayload { Status = SampleEnum.Value2 };

        // Act
        var result = JsonConvert.SerializeObject( payload );

        // Assert
        result.Should().Be( """{"Status":"Value 2"}""" );
    }
    #endregion

    #region Deserialize
    [ Fact ]
    public void Deserialize_ReadsFriendlyName()
    {
        // Arrange
        var json = """{"Status":"Value 2"}""";

        // Act
        var result = JsonConvert.DeserializeObject<FriendlyNameEnumConverterPayload>( json );

        // Assert
        result.Should().BeEquivalentTo( new FriendlyNameEnumConverterPayload { Status = SampleEnum.Value2 } );
    }
    #endregion
}