using System.Linq.Expressions;
using System.Reflection;

namespace LTs.Utils.Reflection;

/// <summary>
///     Extensions for member access <see cref="Expression" /> trees.
/// </summary>
public static class ReflectionExpressionExtensions
{
    /// <summary>
    ///     Gets the property name from a member-access expression.
    /// </summary>
    /// <typeparam name="T">The type that declares the property.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="propertyExpression">An expression that accesses a property.</param>
    /// <returns>The property name.</returns>
    public static string GetPropertyName<T, TProperty>( this Expression<Func<T, TProperty>> propertyExpression )
    {
        ArgumentNullException.ThrowIfNull( propertyExpression );

        var body = propertyExpression.Body;

        if( body is UnaryExpression { Operand: MemberExpression unaryMember }
            && unaryMember.Member is PropertyInfo )
        {
            return unaryMember.Member.Name;
        }

        if( body is MemberExpression { Member: PropertyInfo propertyInfo } )
        {
            return propertyInfo.Name;
        }

        throw new ArgumentException( "Expression must be a property access.", nameof( propertyExpression ) );
    }
}
