using LTs.Utils.Extensions.Strings;

namespace LTs.Utils.test.Extensions.Strings;

public class StringGeneralExtensionsTest
{
    #region TrimEnd
    [ Theory ]
    [ InlineData( "TextRemove", "Remove", "Text" ) ]
    [ InlineData( "TextRemoveEnd", "Remove", "TextRemoveEnd" ) ]
    [ InlineData( "TextRemoveRemove", "Remove", "TextRemove" ) ]
    [ InlineData( "Text", "Remove", "Text" ) ]
    [ InlineData( "Remove", "Remove", "" ) ]
    [ InlineData( "", "Remove", "" ) ]
    [ InlineData( null, "Remove", "" ) ]
    [ InlineData( "TextRemove", null, "TextRemove" ) ]
    public void TrimEnd_ReturnsExpected( string? text, string? remove, string expected )
    {
        // Arrange

        // Act
        var result = text.TrimEnd( remove );

        // Assert
        result.Should().Be( expected );
    }
    #endregion

    #region LowercaseFirst
    [ Theory ]
    [ InlineData( "Text", "text" ) ]
    [ InlineData( "Text1", "text1" ) ]
    [ InlineData( "Text1Text", "text1Text" ) ]
    [ InlineData( "text", "text" ) ]
    [ InlineData( "12abc", "12abc" ) ]
    [ InlineData( "", "" ) ]
    [ InlineData( null, null ) ]
    public void LowercaseFirst_ReturnsExpected( string? text, string? expected )
    {
        // Arrange

        // Act
        var result = text.LowercaseFirst();

        // Assert
        result.Should().Be( expected );
    }
    #endregion

    #region Capitalize
    [ Theory ]
    [ InlineData( "text", "Text" ) ]
    [ InlineData( "text1", "Text1" ) ]
    [ InlineData( "text1Text", "Text1Text" ) ]
    [ InlineData( "Text", "Text" ) ]
    [ InlineData( "12abc", "12abc" ) ]
    [ InlineData( "", "" ) ]
    [ InlineData( null, null ) ]
    public void Capitalize_ReturnsExpected( string? text, string? expected )
    {
        // Arrange

        // Act
        var result = text.Capitalize();

        // Assert
        result.Should().Be( expected );
    }
    #endregion

    #region Find
    [ Theory ]
    // ReSharper disable StringLiteralTypo
    [ InlineData( null, "a", null, null, null ) ] // str null or empty
    [ InlineData( "", "a", null, null, null ) ]
    [ InlineData( "abc", null, null, null, null ) ] // search null or empty
    [ InlineData( "abc", "", null, null, null ) ]
    [ InlineData( "HelloWorld", "world", null, null, 5 ) ] // simple finds (case-insensitive)
    [ InlineData( "TestCase", "TEST", null, null, 0 ) ]
    [ InlineData( "TestCase", "case", null, null, 4 ) ]
    [ InlineData( "abcdefg", "de", 2, null, 3 ) ]    // with start only
    [ InlineData( "abcdefg", "fg", null, 4, null ) ] // with end only (too small → no match)
    [ InlineData( "abcdefg", "fg", 5, 7, 5 ) ]       // with both start/end
    [ InlineData( "abcdefg", "abc", -5, null, 0 ) ]  // negative start clamps to 0
    [ InlineData( "abcdef", "def", null, 100, 3 ) ]  // end beyond length clamps
    // ReSharper restore StringLiteralTypo
    public void Find_ReturnsExpected( string? str, string? search, int? start, int? end, int? expected )
    {
        // Act
        var result = str!.Find( search!, start, end );

        // Assert
        result.Should().Be( expected );
    }

    [ Theory ]
    [ InlineData( "c", 5, 3, "length ('-2') must be a non-negative value. (Parameter 'length')*" ) ]
    [ InlineData( "c", 5, -3, "length ('-8') must be a non-negative value. (Parameter 'length')*" ) ]
    public void Find_InvalidRange_Throws( string search, int? start, int? end, string expectedError )
    {
        // Arrange
        var str = "abcdef";

        // Act
        Action act = () => str.Find( search, start, end );

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage( expectedError );
    }
    #endregion

    #region FindLast
    [ Theory ]
    // ReSharper disable StringLiteralTypo
    [ InlineData( "HelloWorldHello", "hello", null, null, 10 ) ] // simple last‐occurrences (case‐insensitive)
    [ InlineData( "TestCaseTest", "test", null, null, 8 ) ]
    [ InlineData( "abcdefabcgha", "c", null, null, 8 ) ]
    [ InlineData( "abcabcabc", "abc", 3, null, 6 ) ]   // with start only
    [ InlineData( "abcabcabc", "abc", null, 6, 3 ) ]   // with end only
    [ InlineData( "abcabcabc", "abc", 3, 9, 6 ) ]      // with both start/end
    [ InlineData( "abcdabcd", "ab", -5, null, 4 ) ]    // negative start clamps to 0
    [ InlineData( "abcdabcd", "cd", null, 100, 6 ) ]   // end beyond length clamps to str.Length
    [ InlineData( "abcdefg", "h", null, null, null ) ] // not found → null
    [ InlineData( "", "h", null, null, null ) ]        // empty text → null
    [ InlineData( "abcdefg", "", null, null, null ) ]  // empty search → null
    // ReSharper restore StringLiteralTypo
    public void FindLast_ReturnsExpected( string str, string search, int? start, int? end, int? expected )
    {
        // Act
        var result = str.FindLast( search, start, end );

        // Assert
        result.Should().Be( expected );
    }

    [ Theory ]
    [ InlineData( "c", 5, 3, "length ('-2') must be a non-negative value. (Parameter 'length')*" ) ]
    [ InlineData( "c", 5, -3, "length ('-8') must be a non-negative value. (Parameter 'length')*" ) ]
    public void FindLast_InvalidRange_Throws( string search, int? start, int? end, string expectedError )
    {
        // Arrange
        var str = "abcdef";

        // Act
        Action act = () => str.FindLast( search, start, end );

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage( expectedError );
    }
    #endregion

    #region Replace
    [ Theory ]
    [ InlineData( "foo foo foo", "foo", "bar", null, "bar bar bar" ) ] // Replace all when count == null
    [ InlineData( "foo foo foo", "foo", "bar", 2, "bar bar foo" ) ]    // Replace first 2 only
    [ InlineData( "foo foo foo", "foo", "bar", 1, "bar foo foo" ) ]    // Replace first 1 only
    [ InlineData( "foo foo foo", "foo", "bar", 0, "foo foo foo" ) ]    // Replace zero => no change
    [ InlineData( "Abc abc ABC", "abc", "x", null, "Abc x ABC" ) ]     // 5) Case-sensitive: only exact "abc" gets replaced
    [ InlineData( "aaa", "a", "", 2, "a" ) ]                           // Remove two out of three "a"
    [ InlineData( "hello world", "z", "!", null, "hello world" ) ]     // Nothing to replace => unchanged
    [ InlineData( "hello world", "", "!", null, "hello world" ) ]
    [ InlineData( "hello world", null, "b", null, "hello world" ) ]
    [ InlineData( "hello world", "e", null, null, "hello world" ) ]
    [ InlineData( null, "e", "!", null, null ) ]
    public void Replace_ReturnsExpected( string? str, string? search, string? replacement, int? count, string? expected )
    {
        // Act
        var result = str!.Replace( search!, replacement!, count );

        // Assert
        result.Should().Be( expected );
    }
    #endregion

    #region Split
    [ Theory ]
    [ InlineData( "a,b,c", ",", null, new[] { "a", "b", "c" } ) ]
    [ InlineData( "a,b,c", ",", 1, new[] { "a", "b,c" } ) ]
    [ InlineData( "a,b,c", ",", 0, new[] { "a,b,c" } ) ]
    [ InlineData( "abc", "x", null, new[] { "abc" } ) ]
    [ InlineData( "abc", "x", 2, new[] { "abc" } ) ]
    [ InlineData( "abc", "", null, new[] { "a", "b", "c" } ) ]
    [ InlineData( "abc", "", 2, new[] { "a", "b", "c" } ) ]
    [ InlineData( "abc", "", 0, new[] { "abc" } ) ]
    [ InlineData( "abc", null, null, new[] { "abc" } ) ]
    public void Split_ReturnsExpected( string? str, string? separator, int? count, string[] expected )
    {
        // Act
        var result = str!.Split( separator!, count );

        // Assert
        result.Should().Equal( expected );
    }

    [ Theory ]
    [ InlineData( null, ",", null, "Value cannot be null. (Parameter 'str')" ) ]
    [ InlineData( "abc", "", 5, "startIndex cannot be larger than length of string. (Parameter 'startIndex')" ) ]
    public void Split_InvalidParameters_Throws( string? str, string? separator, int? count, string expectedError )
    {
        // Act
        Action act = () => str!.Split( separator!, count );

        // Assert
        act.Should().Throw<Exception>()
           .WithMessage( expectedError );
    }
    #endregion
}