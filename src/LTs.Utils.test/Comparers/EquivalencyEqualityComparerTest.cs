using JetBrains.Annotations;
using LTs.Utils.Comparers;
using LTs.Utils.Comparers.Abstractions;
using Moq;

namespace LTs.Utils.test.Comparers;

public class EquivalencyEqualityComparerTest
{
    #region Equals
    [ Fact ]
    public void IsEqual_Successes()
    {
        // Arrange
        var entity1 = new TestClass { Id = 1, Name = "Test Entity", Date = DateTime.Now };
        var entity2 = entity1;

        var equivalencyComparerMock = new Mock<IEquivalencyComparer<TestClass>>();
        var comparer = new EquivalencyEqualityComparer<TestClass>( equivalencyComparerMock.Object );

        // Act
        _ = comparer.Equals( entity1, entity2 );

        // Assert
        equivalencyComparerMock.Verify( x => x.IsEquivalent( entity1, entity2 ), Times.Once );
    }
    #endregion

    #region GetHashCode
    [ Fact ]
    public void GetHashCode_Successes()
    {
        // Arrange
        var entity1 = new TestClass { Id = 1, Name = "Test Entity", Date = DateTime.Now };

        var equivalencyComparerMock = new Mock<IEquivalencyComparer<TestClass>>();
        var comparer = new EquivalencyEqualityComparer<TestClass>( equivalencyComparerMock.Object );

        // Act
        _ = comparer.GetHashCode( entity1 );

        // Assert
        equivalencyComparerMock.Verify( x => x.GetEquivalencyHash( entity1 ), Times.Once );
    }
    #endregion

    public record TestClass
    {
        [ UsedImplicitly ]
        public required int Id { get; init; }

        [ UsedImplicitly ]
        public required string Name { get; init; }

        [ UsedImplicitly ]
        public required DateTime Date { get; init; }
    }
}