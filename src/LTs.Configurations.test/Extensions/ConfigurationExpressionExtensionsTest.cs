using System.Linq.Expressions;
using LTs.Configurations.Extensions;
using LTs.Configurations.test.Infrastructure;

namespace LTs.Configurations.test.Extensions;

public class ConfigurationExpressionExtensionsTest : BaseTest
{
    public ConfigurationExpressionExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetConfigurationPath
    [ Fact ]
    public void GetConfigurationPath_SinglePropertyAccess_ReturnsPropertyName()
    {
        // Arrange
        Expression<Func<SampleConfiguration, EnumValues>> property = configuration => configuration.EnumValue;

        // Act
        var result = property.GetConfigurationPath();

        // Assert
        result.Should().Be( nameof( SampleConfiguration.EnumValue ) );
    }

    [ Fact ]
    public void GetConfigurationPath_NestedPropertyAccess_ReturnsColonSeparatedPath()
    {
        // Arrange
        Expression<Func<ComplexConfiguration, string>> property = configuration => configuration.Nested.RequiredValue;

        // Act
        var result = property.GetConfigurationPath();

        // Assert
        result.Should().Be( $"{nameof( ComplexConfiguration.Nested )}:{nameof( NestedConfiguration.RequiredValue )}" );
    }

    [ Fact ]
    public void GetConfigurationPath_WhenExpressionIsNotPropertyAccess_Throws()
    {
        // Arrange
        Expression<Func<SampleConfiguration, int>> property = _ => 0;

        // Act
        var act = () => property.GetConfigurationPath();

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "*Expression must be a property access.*" );
    }
    #endregion
}