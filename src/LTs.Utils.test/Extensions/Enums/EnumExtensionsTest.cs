using LTs.Utils.Extensions.Enums;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Extensions.Enums;

public class EnumExtensionsTest : BaseTest
{
    public EnumExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ToEnum
    [ Theory ]
    [ InlineData( "Value1", SampleEnum.Value1 ) ]
    [ InlineData( "value1", SampleEnum.Value1 ) ]
    [ InlineData( "VALUE2", SampleEnum.Value2 ) ]
    public void ToEnum_ParsesEnumMemberName( string text, SampleEnum expected )
    {
        // Arrange

        // Act
        var result = text.ToEnum<SampleEnum>();

        // Assert
        result.Should().Be( expected );
    }

    [ Theory ]
    [ InlineData( null ) ]
    [ InlineData( "" ) ]
    [ InlineData( "   " ) ]
    public void ToEnum_WhenTextIsNullOrWhiteSpace_Throws( string? text )
    {
        // Arrange

        // Act
        var act = () => text.ToEnum<SampleEnum>();

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "Enum value cannot be null or empty.*" );
    }

    [ Fact ]
    public void ToEnum_WhenTextIsNotDefined_Throws()
    {
        // Arrange

        // Act
        var act = () => "NotDefined".ToEnum<SampleEnum>();

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    #endregion

    #region ToValueString
    [ Theory ]
    [ InlineData( SampleEnum.None, "None" ) ]
    [ InlineData( SampleEnum.Value1, "Value1" ) ]
    [ InlineData( SampleEnum.Value3, "Value3" ) ]
    public void ToValueString_ReturnsMemberName( SampleEnum value, string expected )
    {
        // Arrange

        // Act
        var result = value.ToValueString();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToValueString_AndToEnum_RoundTrip()
    {
        // Arrange
        var original = SampleEnum.Value2;

        // Act
        var text = original.ToValueString();
        var result = text.ToEnum<SampleEnum>();

        // Assert
        result.Should().Be( original );
    }
    #endregion
}