using LTs.TestUtils.FluentAssertions;
using Xunit.Sdk;

namespace LTs.TestUtils.test.FluentAssertions;

public class JsonAssertionsExtensionsTest
{
    #region BeSameJsonAs
    [ Fact ]
    public void BeSameJson_SameJson_Successes()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJson_SameJsonDifferentOrder_Successes()
    {
        // Arrange
        var json = @"{ ""age"": 30, ""name"": ""John""  }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJson_DifferentValue_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 15 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has a different value at $.age.*" );
    }

    [ Fact ]
    public void BeSameJson_DifferentNestedValue_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": { ""age"": 15, ""birthday"": ""2010-03-11""} }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": { ""age"": 15, ""birthday"": ""2010-03-12""} }";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has a different value at $.age.birthday.*" );
    }

    [ Fact ]
    public void BeSameJson_ExtraFields_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30, ""field1"": ""value1"" }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has extra property $.field1.*" );
    }
    #endregion

    #region NotBeSameJsonAs
    [ Fact ]
    public void NotBeSameJson_DifferentJson_Throws()
    {
        // Arrange
        var json = @"{ ""parent"": ""John"", ""count"": 1 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().NotBeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotBeSameJson_DifferentValue_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 15 }";

        // Act
        var act = () => json.Should().NotBeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotBeSameJson_SameJson_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().NotBeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "Expected JSON document not to be equivalent to {  \"name\": \"John\",  \"age\": 30}.*" );
    }
    #endregion

    #region BeSameJsonIgnoringExtraFieldsAs
    [ Fact ]
    public void BeSameJsonIgnoringExtraFields_SameJson_Successes()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonIgnoringExtraFields_ExtraFieldsSubject_Success()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30, ""field1"": ""value1"" }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30 }";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonIgnoringExtraFields_ExtraFieldsExpectation_Throws()
    {
        // Arrange
        var json = @"{ ""name"": ""John"", ""age"": 30 }";
        var expectedJson = @"{ ""name"": ""John"", ""age"": 30, ""field1"": ""value1"" }";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document misses property $.field1.*" );
    }
    #endregion
}