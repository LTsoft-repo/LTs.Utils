using System.Linq.Expressions;
using LTs.Utils.Reflection;
using LTs.Utils.test.Infrastructure;

namespace LTs.Utils.test.Reflection;

public class ReflectionExpressionExtensionsTest : BaseTest
{
    public ReflectionExpressionExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetPropertyName
    [ Fact ]
    public void GetPropertyName_PropertyAccess_ReturnsPropertyName()
    {
        // Arrange
        Expression<Func<SourceClass, string>> property = source => source.PropertyA;

        // Act
        var result = property.GetPropertyName();

        // Assert
        result.Should().Be( nameof( SourceClass.PropertyA ) );
    }

    [ Fact ]
    public void GetPropertyName_WhenExpressionIsNotPropertyAccess_Throws()
    {
        // Arrange
        Expression<Func<SourceClass, int>> property = _ => 0;

        // Act
        var act = () => property.GetPropertyName();

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "*Expression must be a property access.*" );
    }
    #endregion
}
