using JetBrains.Annotations;
using LTs.Utils.Reflection;

namespace LTs.Utils.test.Reflection;

public class ReflectionAttributeExtensionsTest
{
    #region HasAttribute (Type)
    [ Fact ]
    public void HasAttribute_AttributeExists_ReturnsTrue()
    {
        // Arrange
        var classType = typeof( ClassWithSomeAttribute );

        // Act
        var hasAttribute = classType.HasAttribute<TestClassAttributeAttribute>();

        // Assert
        hasAttribute.Should().BeTrue();
    }
    #endregion

    #region HasAttribute (PropertyInfo)
    [ Fact ]
    public void HasAttribute_PropertyAttributeExists_ReturnsTrue()
    {
        // Arrange
        var propertyInfo = typeof( ClassWithSomeAttribute )
            .GetProperty( nameof( ClassWithSomeAttribute.Number ) );

        propertyInfo.Should().NotBeNull();

        // Act
        var hasAttribute = propertyInfo!.HasAttribute<TestAttributeAttribute>();

        // Assert
        hasAttribute.Should().BeTrue();
    }

    [ Fact ]
    public void HasAttribute_PropertyAttributeDoesNotExists_ReturnsTrue()
    {
        // Arrange
        var propertyInfo = typeof( ClassWithSomeAttribute )
            .GetProperty( nameof( ClassWithSomeAttribute.Id ) );

        propertyInfo.Should().NotBeNull();

        // Act
        var hasAttribute = propertyInfo!.HasAttribute<TestAttributeAttribute>();

        // Assert
        hasAttribute.Should().BeFalse();
    }
    #endregion

    #region FindPropertyWithAttribute
    [ Fact ]
    public void FindPropertyWithAttribute_AttributeExists_ReturnsPropertyInfo()
    {
        // Arrange
        var classType = typeof( ClassWithSomeAttribute );

        var expectedProperties = classType.GetProperties()
                                          .Where( p => p.Name == nameof( ClassWithSomeAttribute.Number ) )
                                          .ToArray();

        // Act
        var propertyInfos = classType.FindPropertiesWithAttribute<TestAttributeAttribute>()
                                     .ToArray();

        // Assert
        propertyInfos.Should().HaveCount( 1 );
        propertyInfos.Should().Contain( expectedProperties );
    }

    [ Fact ]
    public void FindPropertyWithAttribute_AttributeDoesNotExists_ReturnsEmpty()
    {
        // Arrange
        var classType = typeof( ClassWithNoAttributes );

        // Act
        var propertyInfos = classType.FindPropertiesWithAttribute<TestAttributeAttribute>()
                                     .ToArray();

        // Assert
        propertyInfos.Should().BeEmpty();
    }
    #endregion

    private class ClassWithNoAttributes
    {
        [ UsedImplicitly ]
        public string? Id { get; set; }

        [ UsedImplicitly ]
        public int Number { get; set; }

        [ UsedImplicitly ]
        public int Partition { get; set; }
    }

    [ TestClassAttribute ]
    private class ClassWithSomeAttribute
    {
        [ UsedImplicitly ]
        public string? Id { get; set; }

        [ TestAttribute ]
        public int Number { get; set; }

        [ UsedImplicitly ]
        public int Partition { get; set; }
    }

    private class TestAttributeAttribute : Attribute { }

    private class TestClassAttributeAttribute : Attribute { }
}