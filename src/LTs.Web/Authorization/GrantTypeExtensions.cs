namespace LTs.Web.Authorization;

/// <summary>
///     Extensions for the <see cref="GrantType" /> enum.
/// </summary>
public static class GrantTypeExtensions
{
    /// <summary>
    ///     Converts the <see cref="GrantType" /> to a string that can be used in an identity request.
    /// </summary>
    /// <param name="grantType">The grant type to convert.</param>
    /// <returns>The string representation of the grant type.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static string ToIdentityString( this GrantType grantType )
    {
        if( !GrantTypes.StringRepresentation.TryGetValue( (int)grantType, out var grantTypeString ) )
        {
            throw new ArgumentOutOfRangeException( nameof( grantType ), grantType, null );
        }

        return grantTypeString;
    }

    /// <summary>
    ///     Converts a string to a <see cref="GrantType" />.
    /// </summary>
    /// <param name="grantTypeString">The string to convert.</param>
    /// <returns>The <see cref="GrantType" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static GrantType ToGrantType( this string grantTypeString )
    {
        var grantType = GrantTypes.StringRepresentation.FirstOrDefault( x => x.Value == grantTypeString );

        if( grantType.Key == 0 && string.IsNullOrEmpty( grantType.Value ) )
        {
            throw new ArgumentOutOfRangeException( nameof( grantTypeString ), grantTypeString, null );
        }

        return (GrantType)grantType.Key;
    }
}