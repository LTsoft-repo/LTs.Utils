using LTs.Utils.Reflection;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Reflection;

public class ReflectionHelperTest
{
    #region CreateInstance
    [ Fact ]
    public void CreateInstance_PublicConstructor_Successes()
    {
        // Act
        var result = ReflectionHelper.CreateInstance<TestClass>( "Public Constructor called" );

        // Assert
        result.Should().NotBeNull();
        result.PublicField.Should().Be( "Public Constructor called" );
    }

    [ Fact ]
    public void CreateInstance_PrivateConstructor_Successes()
    {
        // Act
        var result = ReflectionHelper.CreateInstance<TestClass>();

        // Assert
        result.Should().NotBeNull();
        result.PublicField.Should().Be( "Private Constructor called" );
    }

    [ Fact ]
    public void CreateInstance_InternalConstructor_Successes()
    {
        // Act
        var result = ReflectionHelper.CreateInstance<TestClass>( true );

        // Assert
        result.Should().NotBeNull();
        result.PublicField.Should().Be( "Internal Constructor called" );
    }
    #endregion
}