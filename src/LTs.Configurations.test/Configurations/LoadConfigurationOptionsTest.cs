using LTs.Configurations.Configurations;
using LTs.Configurations.test.Infrastructure;

namespace LTs.Configurations.test.Configurations;

public class LoadConfigurationOptionsTest : BaseTest
{ 
    public LoadConfigurationOptionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region SetValue
    [ Fact ]
    public void SetValue_WhenRegisteredTwiceForSameProperty_Throws()
    {
        // Arrange
        var options = new LoadConfigurationOptions<SampleConfiguration>();

        // Act
        var act = () =>
            options
                .SetValue( c => c.EnumValue, _ => EnumValues.Value1 )
                .SetValue( c => c.EnumValue, _ => EnumValues.Value2 );

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "*SetValue was already configured for property 'EnumValue'.*" );
    }

    [ Fact ]
    public void SetValue_ReturnsOptionsInstanceForChaining()
    {
        // Arrange
        var options = new LoadConfigurationOptions<SampleConfiguration>();

        // Act
        var result = options.SetValue( c => c.EnumValue, v => Enum.Parse<EnumValues>( v!, ignoreCase: true ) );

        // Assert
        result.Should().BeSameAs( options );
    }

    [ Fact ]
    public void SetValue_WhenExpressionIsNotPropertyAccess_Throws()
    {
        // Arrange
        var options = new LoadConfigurationOptions<SampleConfiguration>();

        // Act
        var act = () => options.SetValue( _ => EnumValues.Value1, _ => EnumValues.Value2 );

        // Assert
        act.Should().Throw<ArgumentException>()
           .WithMessage( "*Expression must be a property access.*" );
    }
    #endregion
}
