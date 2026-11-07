using System.Linq.Expressions;
using System.Reflection;

namespace LTs.Configurations.Extensions;

/// <summary>
///     Expression helpers for typed configuration load options.
/// </summary>
public static class ConfigurationExpressionExtensions
{
    /// <summary>
    ///     Gets the configuration path from a property access expression.
    /// </summary>
    /// <typeparam name="T">The configuration type.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="propertyExpression">An expression that accesses one or more properties.</param>
    /// <returns>The configuration path.</returns>
    public static string GetConfigurationPath<T, TProperty>( this Expression<Func<T, TProperty>> propertyExpression )
    {
        ArgumentNullException.ThrowIfNull( propertyExpression );

        var members = new List<string>();
        var current = propertyExpression.Body;

        if( current is UnaryExpression unary )
        {
            current = unary.Operand;
        }

        while( current is MemberExpression { Member: PropertyInfo } member )
        {
            members.Add( member.Member.Name );
            current = member.Expression;
        }

        if( members.Count == 0 )
        {
            throw new ArgumentException( "Expression must be a property access.", nameof( propertyExpression ) );
        }

        members.Reverse();

        return string.Join( ":", members );
    }
}
