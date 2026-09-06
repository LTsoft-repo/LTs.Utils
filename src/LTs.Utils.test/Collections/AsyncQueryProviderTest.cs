using System.Linq.Expressions;
using LTs.Utils.Collections;
using Microsoft.EntityFrameworkCore;

namespace LTs.Utils.test.Collections;

public class AsyncQueryProviderTest
{
    #region Constructor
    [ Fact ]
    public void Constructor_WithIQueryProvider_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );

        // Assert
        asyncQueryProvider.Should().NotBeNull();
    }
    #endregion

    #region CreateQuery
    [ Fact ]
    public void CreateQuery_WithExpression_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );
        var expression = Expression.Constant( new[] { 1, 4, 5 } );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );
        var result = asyncQueryProvider.CreateQuery( expression );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo( new[] { 1, 4, 5 } );
    }

    [ Fact ]
    public async Task CreateQuery_WithExpressionT_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );
        var expression = Expression.Constant( new[] { 1, 4, 5 } );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );
        var result = asyncQueryProvider.CreateQuery<int>( expression );

        // Assert
        result.Should().NotBeNull();
        ( await result.ToArrayAsync() ).Should().BeEquivalentTo( new[] { 1, 4, 5 } );
    }
    #endregion

    #region Execute
    [ Fact ]
    public void Execute_WithExpression_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );
        var expression = Expression.Constant( new[] { 1, 4, 5 } );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );
        var result = asyncQueryProvider.Execute( expression );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo( new[] { 1, 4, 5 } );
    }

    [ Fact ]
    public void ExecuteT_WithExpression_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );
        var expression = Expression.Constant( new[] { 1, 4, 5 } );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );
        var result = asyncQueryProvider.Execute<int[]>( expression );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo( new[] { 1, 4, 5 } );
    }
    #endregion

    #region ExecuteAsync
    [ Fact ]
    public void ExecuteAsync_WithExpressionT_Successes()
    {
        // Arrange
        var queryProvider = new AsyncQueryProvider<int>( new EnumerableQuery<int>( new[] { 1, 4, 5 } ) );
        var expression = Expression.Constant( new[] { 1, 4, 5 } );

        // Act
        var asyncQueryProvider = new AsyncQueryProvider<int>( queryProvider );
        var result = asyncQueryProvider.ExecuteAsync<int[]>( expression, CancellationToken.None );

        // Assert
        result.Should().BeEquivalentTo( new[] { 1, 4, 5 } );
    }
    #endregion
}