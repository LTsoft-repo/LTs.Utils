using LTs.Utils.Reflection;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Reflection;

public class ReflectionExtensionsTest
{
    #region GetFieldValue
    [ Fact ]
    public void GetFieldValue_FieldPublic_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetFieldValue<string>( "PublicField" );

        // Assert
        result.Should().Be( "Some value" );
    }

    [ Fact ]
    public void GetFieldValue_FieldProtected_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetFieldValue<string>( "ProtectedField" );

        // Assert
        result.Should().Be( "MyProtectedField" );
    }

    [ Fact ]
    public void GetFieldValue_FieldInternal_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetFieldValue<string>( "InternalField" );

        // Assert
        result.Should().Be( "MyInternalField" );
    }

    [ Fact ]
    public void GetFieldValue_FieldPrivate_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetFieldValue<string>( "privateField" );

        // Assert
        result.Should().Be( "MyPrivateField" );
    }

    [ Fact ]
    public void GetFieldValue_NullObject_Throws()
    {
        // Arrange
        TestClass? obj = null;

        // Act
        Action act = () => obj!.GetFieldValue<string>( "_privateField" );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'obj')" );
    }

    [ Fact ]
    public void GetFieldValue_NullFieldName_Throws()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetFieldValue<string>( null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'fieldName')" );
    }

    [ Theory ]
    [ InlineData( "" ) ]
    [ InlineData( " " ) ]
    [ InlineData( "123" ) ]
    [ InlineData( "With Space" ) ]
    [ InlineData( "NonExistingField" ) ]
    [ InlineData( "PrivateProperty" ) ]
    public void GetFieldValue_InvalidField_Throws( string fieldName )
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetFieldValue<string>( fieldName );

        // Assert
        act.Should().Throw<MissingFieldException>()
           .WithMessage( $"Field not found. (Field '{fieldName}')" );
    }
    #endregion

    #region GetPropertyValue
    [ Fact ]
    public void GetPropertyValue_PropertyPublic_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetPropertyValue<string>( "PublicProperty" );

        // Assert
        result.Should().Be( "MyPublicProperty" );
    }

    [ Fact ]
    public void GetPropertyValue_PropertyProtected_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetPropertyValue<string>( "ProtectedProperty" );

        // Assert
        result.Should().Be( "MyProtectedProperty" );
    }

    [ Fact ]
    public void GetPropertyValue_PropertyInternal_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetPropertyValue<string>( "InternalProperty" );

        // Assert
        result.Should().Be( "MyInternalProperty" );
    }

    [ Fact ]
    public void GetPropertyValue_PropertyPrivate_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetPropertyValue<string>( "PrivateProperty" );

        // Assert
        result.Should().Be( "MyPrivateProperty" );
    }

    [ Fact ]
    public void GetPropertyValue_NullObject_Throws()
    {
        // Arrange
        TestClass? obj = null;

        // Act
        Action act = () => obj!.GetPropertyValue<string>( "_privateProperty" );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'obj')" );
    }

    [ Fact ]
    public void GetPropertyValue_NullPropertyName_Throws()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetPropertyValue<string>( null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'propertyName')" );
    }

    [ Theory ]
    [ InlineData( "" ) ]
    [ InlineData( " " ) ]
    [ InlineData( "123" ) ]
    [ InlineData( "With Space" ) ]
    [ InlineData( "NonExistingField" ) ]
    [ InlineData( "privateField" ) ]
    public void GetPropertyValue_InvalidProperty_Throws( string fieldName )
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetPropertyValue<string>( fieldName );

        // Assert
        act.Should().Throw<MissingFieldException>()
           .WithMessage( $"Property not found. (Property '{fieldName}')" );
    }
    #endregion

    #region GetDataMemberValue
    [ Fact ]
    public void GetDataMemberValue_FieldPublic_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "PublicField" );

        // Assert
        result.Should().Be( "Some value" );
    }

    [ Fact ]
    public void GetDataMemberValue_FieldProtected_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "ProtectedField" );

        // Assert
        result.Should().Be( "MyProtectedField" );
    }

    [ Fact ]
    public void GetDataMemberValue_FieldInternal_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "InternalField" );

        // Assert
        result.Should().Be( "MyInternalField" );
    }

    [ Fact ]
    public void GetDataMemberValue_FieldPrivate_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "privateField" );

        // Assert
        result.Should().Be( "MyPrivateField" );
    }

    [ Fact ]
    public void GetDataMemberValue_PropertyPublic_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "PublicProperty" );

        // Assert
        result.Should().Be( "MyPublicProperty" );
    }

    [ Fact ]
    public void GetDataMemberValue_PropertyProtected_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "ProtectedProperty" );

        // Assert
        result.Should().Be( "MyProtectedProperty" );
    }

    [ Fact ]
    public void GetDataMemberValue_PropertyInternal_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "InternalProperty" );

        // Assert
        result.Should().Be( "MyInternalProperty" );
    }

    [ Fact ]
    public void GetDataMemberValue_PropertyPrivate_Successes()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        var result = obj.GetDataMemberValue<string>( "PrivateProperty" );

        // Assert
        result.Should().Be( "MyPrivateProperty" );
    }

    [ Fact ]
    public void GetDataMemberValue_NullObject_Throws()
    {
        // Arrange
        TestClass? obj = null;

        // Act
        Action act = () => obj!.GetDataMemberValue<string>( "_privateField" );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'obj')" );
    }

    [ Fact ]
    public void GetDataMemberValue_NullDataMemberName_Throws()
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetDataMemberValue<string>( null! );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'dataMemberName')" );
    }

    [ Theory ]
    [ InlineData( "" ) ]
    [ InlineData( " " ) ]
    [ InlineData( "123" ) ]
    [ InlineData( "With Space" ) ]
    [ InlineData( "NonExistingField" ) ]
    public void GetDataMemberValue_InvalidField_Throws( string dataMemberName )
    {
        // Arrange
        var obj = new TestClass( "Some value" );

        // Act
        Action act = () => obj.GetDataMemberValue<string>( dataMemberName );

        // Assert
        act.Should().Throw<MissingFieldException>()
           .WithMessage( $"Data member not found. (Data member '{dataMemberName}')" );
    }
    #endregion
}