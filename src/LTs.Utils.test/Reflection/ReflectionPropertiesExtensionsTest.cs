using LTs.Utils.Reflection;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Reflection;

public class ReflectionPropertiesExtensionsTest : DisposableTest
{
    public ReflectionPropertiesExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region CopyPropertiesFrom
    [ Fact ]
    public void CopyPropertiesFrom_ValidProperties_CopiesProperties()
    {
        // Arrange
        var source = new SourceClass
        {
            PropertyA = "ValueA",
            PropertyB = 42,
            PropertyC = DateTime.Now
        };

        var destination = new DestinationClass
        {
            PropertyA = "OldValueA",
            PropertyB = 0,
            PropertyC = DateTime.MinValue
        };

        var ignoredProperties = new[] { "PropertyB" };

        // Act
        destination.CopyPropertiesFrom( source, ignoredProperties );

        // Assert
        destination.Should().BeEquivalentTo(
            source with
            {
                PropertyB = 0 // PropertyB is ignored.
            } );
    }

    [ Fact ]
    public void CopyPropertiesFrom_NullSource_Throws()
    {
        // Arrange
        SourceClass? source = null;
        var destination = new DestinationClass();

        // Act
        var act = () => destination.CopyPropertiesFrom( source!, [ ] );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'source')" );
    }

    [ Fact ]
    public void CopyPropertiesFrom_NullDestination_Throws()
    {
        // Arrange
        var source = new SourceClass();
        DestinationClass? destination = null;

        // Act
        var act = () => destination!.CopyPropertiesFrom( source, [ ] );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'destination')" );
    }
    #endregion

    #region GetProperty
    [ Fact ]
    public void GetProperty_ExistingProperty_ReturnsValue()
    {
        // Arrange
        var source = new SourceClass
        {
            PropertyA = "ValueA",
            PropertyB = 42,
            PropertyC = DateTime.Now
        };

        // Act
        var propertyAValue = source.GetProperty( "PropertyA" );
        var propertyBValue = source.GetProperty( "PropertyB" );
        var propertyCValue = source.GetProperty( "PropertyC" );

        // Assert
        propertyAValue.Should().NotBeNull();
        propertyAValue!.PropertyType.Should().Be( typeof( string ) );

        propertyBValue.Should().NotBeNull();
        propertyBValue!.PropertyType.Should().Be( typeof( int ) );

        propertyCValue.Should().NotBeNull();
        propertyCValue!.PropertyType.Should().Be( typeof( DateTime ) );
    }

    [ Fact ]
    public void GetProperty_NonExistingProperty_ReturnsNull()
    {
        // Arrange
        var source = new SourceClass();

        // Act
        var propertyValue = source.GetProperty( "NonExistingProperty" );

        // Assert
        propertyValue.Should().BeNull();
    }

    [ Fact ]
    public void GetProperty_NullSource_Throws()
    {
        // Arrange
        SourceClass? source = null;

        // Act
        var act = () => source!.GetProperty( "PropertyC" );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'obj')" );
    }
    #endregion

    #region SetProperty
    [ Fact ]
    public void SetProperty_ExistingProperty_SetsValue()
    {
        // Arrange
        var destination = new DestinationClass
        {
            PropertyA = "OldValueA",
            PropertyB = 0,
            PropertyC = DateTime.MinValue
        };

        var newPropertyAValue = "NewValueA";
        var newPropertyBValue = 100;
        var newPropertyCValue = DateTime.Now;

        // Act
        var result = destination.SetProperty( "PropertyA", newPropertyAValue );

        destination.SetProperty( "PropertyB", newPropertyBValue );
        destination.SetProperty( "PropertyC", newPropertyCValue );

        // Assert
        result.Should().BeTrue();
        destination.PropertyA.Should().Be( newPropertyAValue );
        destination.PropertyB.Should().Be( newPropertyBValue );
        destination.PropertyC.Should().Be( newPropertyCValue );
    }

    [ Fact ]
    public void SetProperty_NonExistingProperty_ReturnsFalse()
    {
        // Arrange
        var destination = new DestinationClass();

        // Act
        var result = destination.SetProperty( "NonExistingProperty", "SomeValue" );

        // Assert
        result.Should().BeFalse();
    }

    [ Fact ]
    public void SetProperty_WrongType_ReturnsFalse()
    {
        // Arrange
        var destination = new DestinationClass();
        // Act
        var result = destination.SetProperty( "PropertyA", 123 );

        // Assert
        result.Should().BeFalse();
    }

    [ Fact ]
    public void SetProperty_NullSource_Throws()
    {
        // Arrange
        DestinationClass? destination = null;
        // Act
        var act = () => destination!.SetProperty( "PropertyA", "SomeValue" );

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage( "Value cannot be null. (Parameter 'obj')" );
    }
    #endregion
}