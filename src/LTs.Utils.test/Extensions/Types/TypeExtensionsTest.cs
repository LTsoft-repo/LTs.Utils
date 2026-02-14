using JetBrains.Annotations;
using LTs.Utils.Extensions.Types;

namespace LTs.Utils.test.Extensions.Types;

public class TypeExtensionsTest
{
    #region IsBasicType
    [ Theory ]
    [ InlineData( typeof( int ) ) ]
    [ InlineData( typeof( double ) ) ]
    [ InlineData( typeof( bool ) ) ]
    [ InlineData( typeof( char ) ) ]
    [ InlineData( typeof( decimal ) ) ]
    [ InlineData( typeof( float ) ) ]
    [ InlineData( typeof( byte ) ) ]
    [ InlineData( typeof( short ) ) ]
    [ InlineData( typeof( long ) ) ]
    [ InlineData( typeof( sbyte ) ) ]
    [ InlineData( typeof( ushort ) ) ]
    [ InlineData( typeof( uint ) ) ]
    [ InlineData( typeof( ulong ) ) ]
    [ InlineData( typeof( Enum1 ) ) ]
    public void IsBasicType_BasicTypes_ReturnsTrue( Type type )
    {
        // Arrange

        // Act
        var result = type.IsBasicType();

        // Assert
        result.Should().BeTrue();
    }

    [ Theory ]
    [ InlineData( typeof( int[] ) ) ]
    [ InlineData( typeof( Class1 ) ) ]
    [ InlineData( typeof( Record1 ) ) ]
    [ InlineData( typeof( Class1[] ) ) ]
    [ InlineData( typeof( Record1[] ) ) ]
    [ InlineData( typeof( IEnumerable<string> ) ) ]
    [ InlineData( typeof( IEnumerable<Class1> ) ) ]
    [ InlineData( typeof( IEnumerable<Record1> ) ) ]
    [ InlineData( typeof( Array ) ) ]
    [ InlineData( typeof( Dictionary<string, int> ) ) ]
    [ InlineData( typeof( List<int> ) ) ]
    public void IsBasicType_CustomClasses_ReturnsFalse( Type type )
    {
        // Arrange

        // Act
        var result = type.IsBasicType();

        // Assert
        result.Should().BeFalse();
    }
    #endregion

    #region IsAnonymousType
    [ Fact ]
    public void IsAnonymousType_AnonymousType_ReturnsTrue()
    {
        // Arrange
        var obj1 = new { Something = "something" };
        var obj2 = new { };

        // Act
        var resultObj1 = obj1.GetType().IsAnonymousType();
        var resultObj2 = obj2.GetType().IsAnonymousType();

        // Assert
        resultObj1.Should().BeTrue();
        resultObj2.Should().BeTrue();
    }

    [ Fact ]
    public void IsAnonymousType_OtherType_ReturnsFalse()
    {
        // Arrange
        var obj = new object();
        var class1 = new Class1();
        var record1 = new Record1();

        // Act
        var resultObj = obj.GetType().IsAnonymousType();
        var resultClass1 = class1.GetType().IsAnonymousType();
        var resultRecord1 = record1.GetType().IsAnonymousType();

        // Assert
        resultObj.Should().BeFalse();
        resultClass1.Should().BeFalse();
        resultRecord1.Should().BeFalse();
    }
    #endregion

    #region IsDefault
    [ Fact ]
    public void IsDefault_NullOrDefault_ReturnsTrue()
    {
        // Arrange
        object? objNull = null;
        Class1 class1Null = null!;
        Record1 record1Null = null!;

        object? objDefault = default;
        var anonymousDefault = new { };
        Class1 class1Default = new();
        Record1 record1Default = new();

        // Act
        var resultObjNull = objNull.IsDefault();
        var resultClass1Null = class1Null.IsDefault();
        var resultRecord1Null = record1Null.IsDefault();

        var resultObjDefault = objDefault.IsDefault();
        var resultAnonymousDefault = anonymousDefault.IsDefault();
        var resultClass1Default = class1Default.IsDefault();
        var resultRecord1Default = record1Default.IsDefault();

        // Assert
        resultObjNull.Should().BeTrue();
        resultClass1Null.Should().BeTrue();
        resultRecord1Null.Should().BeTrue();

        resultObjDefault.Should().BeTrue();
        resultAnonymousDefault.Should().BeTrue();
        resultClass1Default.Should().BeTrue();
        resultRecord1Default.Should().BeTrue();
    }

    [ Fact ]
    public void IsDefault_NonDefault_ReturnsFalse()
    {
        // Arrange
        var obj = new { Something = "something" };
        Class1 class1 = new() { Value = 2 };
        Record1 record1 = new() { Value = 2 };

        // Act
        var resultObj = obj.IsDefault();
        var resultClass1 = class1.IsDefault();
        var resultRecord1 = record1.IsDefault();

        // Assert
        resultObj.Should().BeFalse();
        resultClass1.Should().BeFalse();
        resultRecord1.Should().BeFalse();
    }
    #endregion

    private enum Enum1
    {
        [ UsedImplicitly ] Value1,
        [ UsedImplicitly ] Value2
    }

    private class Class1
    {
        [ UsedImplicitly ]
        public int Value { get; set; }
    }

    private class Record1
    {
        [ UsedImplicitly ]
        public int Value { get; set; }
    }
}