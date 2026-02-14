using FluentAssertions.Equivalency;
using LTs.TestUtils.FluentAssertions;
using LTs.Utils.Collections;
using Xunit.Sdk;

namespace LTs.TestUtils.test.FluentAssertions;

public class GenericCollectionAssertionsExtensionsTest
{
    #region ContainEquivalentSubset - generic (with options)
    [ Fact ]
    public void ContainEquivalentSubsetWithOptions_WithDifferentSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "A", "B", "C" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetWithOptions_SameSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "b", "c" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetWithOptions_PartialSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "B" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetWithOptions_NonExistingSubsetElement_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "A", "D" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection {\"a\", \"b\", \"c\"} to contain an equivalent of \"D\", but no match was found.*" );
    }

    [ Fact ]
    public void ContainEquivalentSubsetWithOptions_EmptySubset_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = Array.Empty<string>();

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Cannot assert a subset of an empty collection." );
    }
    #endregion

    #region ContainEquivalentSubset - generic
    [ Fact ]
    public void ContainEquivalentSubset_WithSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "b" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubset_NonExistingSubsetElement_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "d" };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection {\"a\", \"b\", \"c\"} to contain an equivalent of \"d\", but no match was found.*" );
    }
    #endregion

    #region ContainEquivalentSubset - Grouping (with options)
    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_WithDifferentSubset_Successes()
    {
        // Arrange
        IGrouping<string, string>[] subject =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "b", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( "c", [ "c1", "c2", "c3" ] )
        ];


        IGrouping<string, string>[] expected =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "B", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( "c", [ "c1", "C2", "C3" ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_SameSubset_Successes()
    {
        // Arrange
        IGrouping<string, int>[] subject =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
            new Grouping<string, int>( "c", [ 6, 7, 8 ] )
        ];

        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int>[] expected =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
            new Grouping<string, int>( "c", [ 6, 7, 8 ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, options => options );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_PartialSubset_Successes()
    {
        // Arrange
        IGrouping<string, int>[] subject =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
            new Grouping<string, int>( "c", [ 6, 7, 8 ] )
        ];

        IGrouping<string, int>[] expected =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, options => options );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_NonExistingSubsetKey_Throws()
    {
        // Arrange
        IGrouping<string, string>[] subject =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "b", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( "c", [ "c1", "c2", "c3" ] )
        ];

        IGrouping<string, string>[] expected =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "D", [ "d1", "d2", "d3" ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected Keys {\"a\", \"b\", \"c\"} to contain equivalent of \"D\", but no match was found.*" );
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_NonExistingSubsetElement_Throws()
    {
        // Arrange
        IGrouping<string, string>[] subject =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "b", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( "c", [ "c1", "c2", "c3" ] )
        ];

        IGrouping<string, string>[] expected =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "B", [ "b1", "X2", "b3" ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected group of Key \"B\" with collection {\"b1\", \"b2\", \"b3\"} " +
                         "to contain an equivalent of \"X2\", but no match was found.*" );
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_EmptySubset_Throws()
    {
        // Arrange
        IGrouping<string, string>[] subject =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "b", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( "c", [ "c1", "c2", "c3" ] )
        ];

        // ReSharper disable once CoVariantArrayConversion
        IGrouping<string, string>[] expected = [ ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitive, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Cannot assert a subset of an empty collection." );
    }

    [ Fact ]
    public void ContainEquivalentSubsetGroupingWithOptions_WithNullKey_Successes()
    {
        // Arrange
        IGrouping<string?, string>[] subject =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "b", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( null!, [ "c1", "c2", "c3" ] )
        ];

        IGrouping<string?, string>[] expected =
        [
            new Grouping<string, string>( "a", [ "a1", "a2", "a3" ] ),
            new Grouping<string, string>( "B", [ "b1", "b2", "b3" ] ),
            new Grouping<string, string>( null!, [ "c1", "C2", "C3" ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected, OptionsCaseInsensitiveForNullable, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region ContainEquivalentSubset - Grouping
    //[ Fact ]
    //public void ContainEquivalentSubsetGrouping_WithSubset_Successes()
    //{
    //    // Arrange
    //    var subject = new[]
    //    {
    //        new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
    //        new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
    //        new Grouping<string, int>( "c", [ 6, 7, 8 ] )
    //    };

    //    var expected = new[]
    //    {
    //        new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
    //        new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
    //        new Grouping<string, int>( "c", [ 6, 7, 8 ] )
    //    };

    //    // Act
    //    Action act = () => subject.Should().ContainEquivalentSubset( expected );

    //    // Assert
    //    act.Should().NotThrow();
    //}

    [ Fact ]
    public void ContainEquivalentSubsetGrouping_WithSubset_Successes()
    {
        // Arrange
        IGrouping<string, int>[] subject =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
            new Grouping<string, int>( "c", [ 6, 7, 8 ] )
        ];

        IGrouping<string, int>[] expected =
        [
            new Grouping<string, int>( "a", [ 1, 2, 3 ] ),
            new Grouping<string, int>( "b", [ 4, 5, 3 ] ),
            new Grouping<string, int>( "c", [ 6, 7, 8 ] )
        ];

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetGrouping_WithNullElement_Successes()
    {
        // Arrange
        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int?>[] subject = new Grouping<string, int?>[]
        {
            new( "a", new int?[] { 1, 2, 3 } ),
            new( "b", new int?[] { 4, null, 3 } ),
            new( "c", new int?[] { 6, 7, 8 } )
        };

        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int?>[] expected = new Grouping<string, int?>[]
        {
            new( "b", new int?[] { 4, null, 3 } )
        };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainEquivalentSubsetGrouping_NonExistingSubsetKey_Throws()
    {
        // Arrange
        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int>[] subject = new Grouping<string, int>[]
        {
            new( "a", new[] { 1, 2, 3 } ),
            new( "b", new[] { 4, 5, 3 } ),
            new( "c", new[] { 6, 7, 8 } )
        };

        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int>[] expected = new Grouping<string, int>[]
        {
            new( "d", new[] { 9, 10, 11 } )
        };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected Keys {\"a\", \"b\", \"c\"} to contain equivalent of \"d\", but no match was found.*" );
    }

    [ Fact ]
    public void ContainEquivalentSubsetGrouping_NonExistingSubsetElement_Throws()
    {
        // Arrange
        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int>[] subject = new Grouping<string, int>[]
        {
            new( "a", new[] { 1, 2, 3 } ),
            new( "b", new[] { 4, 5, 3 } ),
            new( "c", new[] { 6, 7, 8 } )
        };

        // ReSharper disable once CoVariantArrayConversion
        // ReSharper disable once RedundantArrayCreationExpression
        IGrouping<string, int>[] expected = new Grouping<string, int>[]
        {
            new( "b", new[] { 4, 99, 3 } )
        };

        // Act
        Action act = () => subject.Should().ContainEquivalentSubset( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected group of Key \"b\" with collection {4, 5, 3} " +
                         "to contain an equivalent of 99, but no match was found.*" );
    }
    #endregion

    #region ContainExactlyEquivalent
    [ Fact ]
    public void ContainExactlyEquivalent_WithSameElements_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "b", "c" };

        // Act
        Action act = () => subject.Should().ContainExactlyEquivalent( expected );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void ContainExactlyEquivalent_WithExtraElements_Throw()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "b" };

        // Act
        Action act = () => subject.Should().ContainExactlyEquivalent( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection to not contain extra items, but found an unexpected \"c\" in {\"a\", \"b\"}.*" );
    }

    [ Fact ]
    public void ContainExactlyEquivalent_NonExistingElement_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b" };
        var expected = new[] { "a", "d" };

        // Act
        Action act = () => subject.Should().ContainExactlyEquivalent( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection {\"a\", \"b\"} to contain an equivalent of \"d\", but no match was found.*" );
    }
    #endregion

    #region NotContainEquivalentInSubset (with options)
    [ Fact ]
    public void NotContainEquivalentInSubsetWithOptions_WithDifferentSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "D", "E", "F" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotContainEquivalentInSubsetWithOptions_SameSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "d", "e", "f" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotContainEquivalentInSubsetWithOptions_ExistingSubsetElement_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection to not contain equivalent of \"a\", but a match was found.*" );
    }

    [ Fact ]
    public void NotContainEquivalentInSubsetWithOptions_ExistingSubsetElement2_Throws()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "A", "D" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection to not contain equivalent of \"A\", but a match was found.*" );
    }

    [ Fact ]
    public void NotContainEquivalentInSubsetWithOptions_EmptySubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = Array.Empty<string>();

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected, OptionsCaseInsensitive );

        // Assert
        act.Should().NotThrow();
    }
    #endregion

    #region NotContainEquivalentInSubset
    [ Fact ]
    public void NotContainEquivalentInSubset_NotInSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "d", "x" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected );

        // Assert
        act.Should().NotThrow();
    }

    [ Fact ]
    public void NotContainEquivalentInSubset_WithinSubset_Successes()
    {
        // Arrange
        var subject = new[] { "a", "b", "c" };
        var expected = new[] { "a", "d" };

        // Act
        Action act = () => subject.Should().NotContainEquivalentInSubset( expected );

        // Assert
        act.Should().Throw<XunitException>()
           .WithMessage( "Expected collection to not contain equivalent of \"a\", but a match was found.*" );
    }
    #endregion

    private EquivalencyAssertionOptions<string> OptionsCaseInsensitive( EquivalencyAssertionOptions<string> config )
        => config.Using<string>( ctx => ctx.Subject.Should().MatchEquivalentOf( ctx.Expectation ) )
                 .WhenTypeIs<string>();

    private EquivalencyAssertionOptions<string?> OptionsCaseInsensitiveForNullable( EquivalencyAssertionOptions<string?> config )
        => config.Using<string?>( ctx =>
                     {
                         ctx.Subject.Should().MatchEquivalentOf( ctx.Expectation );
                     } )
                 .WhenTypeIs<string?>();
}