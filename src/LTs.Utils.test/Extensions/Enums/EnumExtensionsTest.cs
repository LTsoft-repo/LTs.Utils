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
    [ InlineData( "Value 1", SampleEnum.Value1 ) ]
    [ InlineData( "Value One", FriendlyNameSampleEnum.ValueOne ) ]
    [ InlineData( "value one", FriendlyNameSampleEnum.ValueOne ) ]
    [ InlineData( "Long Running Process", FriendlyNameSampleEnum.LongRunningProcess ) ]
    [ InlineData( "Value With Number 31 And Something Else", FriendlyNameSampleEnum.ValueWithNumber31AndSomethingElse ) ]
    public void ToEnum_CorrectStrings_Successes<T>( string text, T expected )
        where T : struct, Enum
    {
        // Arrange

        // Act
        var result = text.ToEnum<T>();

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

    #region ToEnum (Type)
    [ Theory ]
    [ InlineData( "Value 2", SampleEnum.Value2 ) ]
    [ InlineData( "Value2", SampleEnum.Value2 ) ]
    public void ToEnum_WithEnumType_ParsesFriendlyOrMemberName( string text, SampleEnum expected )
    {
        // Arrange

        // Act
        var result = text.ToEnum( typeof( SampleEnum ) );

        // Assert
        result.Should().Be( expected );
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
    public void ToValueString_OnEnum_ReturnsSameAsGeneric()
    {
        // Arrange
        Enum value = SampleEnum.Value3;

        // Act
        var result = value.ToValueString();

        // Assert
        result.Should().Be( SampleEnum.Value3.ToValueString() );
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

    #region ToFriendlyName
    [ Theory ]
    [ InlineData( SampleEnum.None, "None" ) ]
    [ InlineData( SampleEnum.Value1, "Value 1" ) ]
    [ InlineData( SampleEnum.Value2, "Value 2" ) ]
    [ InlineData( SampleEnum.Value3, "Value 3" ) ]
    [ InlineData( FriendlyNameSampleEnum.ValueOne, "Value One" ) ]
    [ InlineData( FriendlyNameSampleEnum.LongRunningProcess, "Long Running Process" ) ]
    [ InlineData( FriendlyNameSampleEnum.ValueWithNumber31AndSomethingElse, "Value With Number 31 And Something Else" ) ]
    public void ToFriendlyName_ReturnsExpectedText<T>( T value, string expected )
        where T : struct, Enum
    {
        // Arrange

        // Act
        var result = value.ToFriendlyName();

        // Assert
        result.Should().Be( expected );
    }

    [ Fact ]
    public void ToFriendlyName_OnEnum_ReturnsSameAsGeneric()
    {
        // Arrange
        Enum value = SampleEnum.Value2;

        // Act
        var result = value.ToFriendlyName();

        // Assert
        result.Should().Be( SampleEnum.Value2.ToFriendlyName() );
    }

    [ Theory ]
    [ InlineData( SampleEnum.Value2 ) ]
    [ InlineData( FriendlyNameSampleEnum.LongRunningProcess ) ]
    public void ToFriendlyName_AndToEnum_RoundTrip<T>( T original )
        where T : struct, Enum
    {
        // Arrange

        // Act
        var text = original.ToFriendlyName();
        var result = text.ToEnum<T>();

        // Assert
        result.Should().Be( original );
    }
    #endregion
}