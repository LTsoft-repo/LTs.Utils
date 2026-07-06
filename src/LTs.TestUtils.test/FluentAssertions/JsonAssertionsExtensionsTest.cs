using LTs.TestUtils.FluentAssertions;
using LTs.TestUtils.Tests;
using Xunit.Sdk;

#pragma warning disable IDE0290 // Primary constructor should be used
#pragma warning disable CS0618  // Type or member is obsolete

namespace LTs.TestUtils.test.FluentAssertions;

public class JsonAssertionsExtensionsTest : BaseTest
{
    public JsonAssertionsExtensionsTest( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region BeSameJsonAs
    [ Fact ]
    public void BeSameJsonAs_SameJson_Successes()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30 }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_SameJsonDifferentOrder_Successes()
    {
        // Arrange
        var json = """{ "age": 30, "name": "John"  }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_DifferentValue_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": 15 }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has a different value at $.age.*" );
    }

    [ Fact ]
    public void BeSameJsonAs_DifferentNestedValue_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": { "age": 15, "birthday": "2010-03-11"} }""";
        var expectedJson = """{ "name": "John", "age": { "age": 15, "birthday": "2010-03-12"} }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has a different value at $.age.birthday.*" );
    }

    [ Fact ]
    public void BeSameJsonAs_ExtraFields_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30, "field1": "value1" }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has extra property $.field1.*" );
    }

    [ Fact ]
    public void BeSameJsonAs_ExcludedJsonPaths_Successes()
    {
        // Arrange
        var json = """{ "id": "1", "name": "John", "metadata": { "version": "2", "created": "today" } }""";
        var expectedJson = """{ "id": "2", "name": "John", "metadata": { "version": "3", "created": "today" } }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson, [ "id", "metadata.version" ] );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_ExcludedJsonPathsOptions_Successes()
    {
        // Arrange
        var json = """{ "id": "1", "name": "John", "metadata": { "version": "2", "created": "today" } }""";
        var expectedJson = """{ "id": "2", "name": "John", "metadata": { "version": "3", "created": "today" } }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.Exclude( "id" )
                                                                      .Exclude( "metadata.version" ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_MultipleMismatches_ThrowsAllMismatches()
    {
        // Arrange
        var json = """{ "name": "John", "age": 15, "address": { "city": "Miami" } }""";
        var expectedJson = """{ "name": "Jane", "age": 30, "address": { "city": "Orlando" } }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .Which
           .Message.Should().Contain( "JSON document has 3 mismatches:" )
           .And.Contain( "JSON document has a different value at $.name." )
           .And.Contain( "JSON document has a different value at $.age." )
           .And.Contain( "JSON document has a different value at $.address.city." );
    }

    [ Fact ]
    public void BeSameJsonAs_ExcludedJsonPathsWithRemainingMismatch_ThrowsRemainingMismatch()
    {
        // Arrange
        var json = """{ "id": "1", "name": "John", "metadata": { "version": "2" } }""";
        var expectedJson = """{ "id": "2", "name": "Jane", "metadata": { "version": "3" } }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson, [ "id", "metadata.version" ] );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document has a different value at $.name.*" );
    }

    [ Fact ]
    public void BeSameJsonAs_OptionsIgnoringExtraFields_Successes()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30, "field1": "value1" }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";
        var options = new JsonAssertionOptions { IgnoreExtraFields = true };

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson, options );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_OptionsIgnoringExtraFieldsAndExcludedJsonPaths_Successes()
    {
        // Arrange
        var json = """{ "id": "1", "name": "John", "metadata": { "version": "2" }, "field1": "value1" }""";
        var expectedJson = """{ "id": "2", "name": "John", "metadata": { "version": "3" } }""";

        var options = new JsonAssertionOptions
        {
            IgnoreExtraFields = true,
            ExcludedJsonPaths = [ "id", "metadata.version" ]
        };

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson, options );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsIgnoringExtraFieldsAndExcludedJsonPaths_Successes()
    {
        // Arrange
        var json = """
                   {
                     "id": "1",
                     "name": "John",
                     "metadata": {
                       "version": "2"
                     },
                     "field1": "value1"
                   }
                   """;

        var expectedJson = """
                           {
                             "id": "2",
                             "name": "John",
                             "metadata": {
                               "version": "3"
                             }
                           }
                           """;

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.IgnoringExtraFields()
                                                                      .Exclude( "id" )
                                                                      .Exclude( "metadata.version" ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsIgnoringExtraFieldsInArray_Successes()
    {
        // Arrange
        var json = """
                   {
                     "id": "1",
                     "name": "John",
                     "metadata": {
                       "version": "2"
                     },
                     "values": [
                       { "id": "1", "value": "A" },
                       { "id": "2", "value": "B" }
                     ],
                     "field1": "value1"
                   }
                   """;

        var expectedJson = """
                           {
                             "id": "2",
                             "name": "John",
                             "metadata": {
                               "version": "3"
                             },
                             "values": [
                               { "id": "1", "value": "A" },
                               { "id": "4", "value": "B" }
                             ]
                           }
                           """;

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.IgnoringExtraFields()
                                                                      .Exclude( "id" )
                                                                      .Exclude( "metadata.version" )
                                                                      .Exclude( "values[*].id" ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsWithMatcher_Successes()
    {
        // Arrange
        var json = """{ "field1": "actual value", "name": "John" }""";
        var expectedJson = """{ "field1": "actual value", "name": "John" }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.WithMatcher( "field1",
                                                                                    value => value.Should().Contain( "value" ) ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsWithMatcherWildcard_Successes()
    {
        // Arrange
        var json = """{ "values": [ { "id": "value-1" }, { "id": "value-2" } ] }""";
        var expectedJson = """{ "values": [ { "id": "some-value-3" }, { "id": "some-value-4" } ] }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.WithMatcher( "values[*].id",
                                                                                    value => value.Should().Contain( "value" ) ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsWithMatcherWildcard_ThrowsMatcherMismatches()
    {
        // Arrange
        var json = """{ "values": [ { "id": "value-1" }, { "id": "value-2" } ] }""";
        var expectedJson = """{ "values": [ { "id": "expected-1" }, { "id": "expected-2" } ] }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.WithMatcher( "values[*].id",
                                                                                    value => value.Should().Contain( "value" ) ) );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .Which
           .Message.Should().Contain( "JSON document has 2 mismatches:" )
           .And.Contain( "JSON document expectation matcher failed at $.values[0].id." )
           .And.Contain( "JSON document expectation matcher failed at $.values[1].id." );
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsWithMatcher_ThrowsMatcherMismatch()
    {
        // Arrange
        var json = """{ "field1": "actual value", "name": "John" }""";
        var expectedJson = """{ "field1": "actual text", "name": "Jane" }""";

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.WithMatcher( "field1",
                                                                                    value => value.Should().Contain( "value" ) ) );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .Which
           .Message.Should().Contain( "JSON document has 2 mismatches:" )
           .And.Contain( "JSON document has a different value at $.name." )
           .And.Contain( "JSON document expectation matcher failed at $.field1." )
           .And.Contain( "to contain \"value\"" );
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsExclusionsFieldAndMatcherWithExcludedField_Successes()
    {
        // Arrange
        var json = """
                   {
                     "id": "1",
                     "metadata": {
                       "version": "2"
                     },
                     "values": [
                       { "id": "1", "value": "A" },
                       { "id": "2", "value": "B" }
                     ],
                     "field1": "actual value"
                   }
                   """;

        var expectedJson = """
                           {
                             "id": "2",
                             "metadata": {
                               "version": "3"
                             },
                             "values": [
                               { "id": "3", "value": "A, A1, A2" },
                               { "id": "4", "value": "B" }
                             ],
                             "field1": "actual value"
                           }
                           """;

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.Exclude( "id" )
                                                                      .Exclude( "metadata.version" )
                                                                      .Exclude( "values" )
                                                                      .WithMatcher( "values[0].value",
                                                                                    value => value.Should().Contain( "A" ) ) );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonAs_FluentOptionsIgnoringExtraFieldsExclusionsAndMatcher_Successes()
    {
        // Arrange
        var json = """
                   {
                     "id": "1",
                     "metadata": {
                       "version": "2"
                     },
                     "values": [
                       { "id": "1", "value": "A" },
                       { "id": "2", "value": "B" }
                     ],
                     "field1": "actual value",
                     "extra": "ignored"
                   }
                   """;

        var expectedJson = """
                           {
                             "id": "2",
                             "metadata": {
                               "version": "3"
                             },
                             "values": [
                               { "id": "3", "value": "A" },
                               { "id": "4", "value": "B" }
                             ],
                             "field1": "actual value 2"
                           }
                           """;

        // Act
        var act = () => json.Should().BeSameJsonAs( expectedJson,
                                                    options => options.IgnoringExtraFields()
                                                                      .Exclude( "id" )
                                                                      .Exclude( "metadata.version" )
                                                                      .Exclude( "values[*].id" )
                                                                      .WithMatcher( "field1",
                                                                                    value => value.Should().Contain( "value" ) ) );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region NotBeSameJsonAs
    [ Fact ]
    public void NotBeSameJsonAs_DifferentJson_Throws()
    {
        // Arrange
        var json = """{ "parent": "John", "count": 1 }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().NotBeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotBeSameJsonAs_DifferentValue_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30 }""";
        var expectedJson = """{ "name": "John", "age": 15 }""";

        // Act
        var act = () => json.Should().NotBeSameJsonAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotBeSameJsonAs_SameJson_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30 }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

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
        var json = """{ "name": "John", "age": 30 }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonIgnoringExtraFields_ExtraFieldsSubject_Success()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30, "field1": "value1" }""";
        var expectedJson = """{ "name": "John", "age": 30 }""";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void BeSameJsonIgnoringExtraFields_ExtraFieldsExpectation_Throws()
    {
        // Arrange
        var json = """{ "name": "John", "age": 30 }""";
        var expectedJson = """{ "name": "John", "age": 30, "field1": "value1" }""";

        // Act
        var act = () => json.Should().BeSameJsonIgnoringExtraFieldsAs( expectedJson );

        // Assert
        act.Should().ThrowExactly<XunitException>()
           .WithMessage( "JSON document misses property $.field1.*" );
    }
    #endregion
}