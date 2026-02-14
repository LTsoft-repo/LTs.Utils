using FluentAssertions;
using FluentAssertions.Json;
using FluentAssertions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for JSON equivalencies.
/// </summary>
public static class JsonAssertionsExtensions
{
    /// <summary>
    ///     Returns an <see cref="JTokenAssertions" /> object that can be used to assert the current <see cref="JToken" />.
    /// </summary>
    [ Pure ]
    public static JTokenAssertions Should( this JToken? jToken )
        => new( jToken );

    /// <summary>
    ///     Returns an <see cref="JTokenAssertions" /> object that can be used to assert the current <see cref="JObject" />.
    /// </summary>
    [ Pure ]
    public static JTokenAssertions Should( this JObject? jObject )
        => new( jObject );

    /// <summary>
    ///     Returns an <see cref="JTokenAssertions" /> object that can be used to assert the current <see cref="JValue" />.
    /// </summary>
    [ Pure ]
    public static JTokenAssertions Should( this JValue? jValue )
        => new( jValue );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<TAssertions> BeSameJsonAs<TAssertions>(
        this StringAssertions<TAssertions> assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
    {
        var subject = assertions.Subject.ParseWithDatesAsString();
        var expected = expectation;

        subject.Should().BeSameJsonAs( expected, because, becauseArgs );

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<JTokenAssertions> BeSameJsonAs(
        this JTokenAssertions assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var stringSubject = assertions.Subject.ToString( Formatting.None );
        var subject = stringSubject.ParseWithDatesAsString();
        var expected = expectation.ParseWithDatesAsString();

        return subject.Should().BeSameJsonAs( expected, because, becauseArgs );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<JTokenAssertions> BeSameJsonAs(
        this JTokenAssertions assertions,
        JToken expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var subject = assertions.Subject;
        var result = subject.Should().BeEquivalentTo( expectation, because, becauseArgs );

        return result;
    }

    /// <summary>
    ///     Asserts that the JSON string is not equivalent to the expected JSON string.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<TAssertions> NotBeSameJsonAs<TAssertions>(
        this StringAssertions<TAssertions> assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
    {
        var subject = assertions.Subject.ParseWithDatesAsString();
        var expected = expectation;

        subject.Should().NotBeSameJsonAs( expected, because, becauseArgs );

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }

    /// <summary>
    ///     Asserts that the JSON string is not equivalent to the expected JSON string.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public static AndConstraint<JTokenAssertions> NotBeSameJsonAs(
        this JTokenAssertions assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var subject = assertions.Subject;
        var expected = expectation.ParseWithDatesAsString();

        return subject.Should().NotBeSameJsonAs( expected, because, becauseArgs );
    }

    /// <summary>
    ///     Asserts that the JSON string is not equivalent to the expected JSON string.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public static AndConstraint<JTokenAssertions> NotBeSameJsonAs(
        this JTokenAssertions assertions,
        JToken expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var subject = assertions.Subject;
        var result = subject.Should().NotBeEquivalentTo( expectation, because, becauseArgs );

        return result;
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string ignoring additional fields.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public static AndConstraint<TAssertions> BeSameJsonIgnoringExtraFieldsAs<TAssertions>(
        this StringAssertions<TAssertions> assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
    {
        var subject = assertions.Subject.ParseWithDatesAsString();
        var expected = expectation;

        subject.Should().BeSameJsonIgnoringExtraFieldsAs( expected, because, becauseArgs );

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string ignoring additional fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public static AndConstraint<JTokenAssertions> BeSameJsonIgnoringExtraFieldsAs(
        this JTokenAssertions assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var subject = assertions.Subject;
        var expected = expectation.ParseWithDatesAsString();

        return subject.Should().BeSameJsonIgnoringExtraFieldsAs( expected, because, becauseArgs );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string ignoring additional fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    [ UsedImplicitly ]
    public static AndConstraint<JTokenAssertions> BeSameJsonIgnoringExtraFieldsAs(
        this JTokenAssertions assertions,
        JToken expectation,
        string because = "",
        params object[] becauseArgs )
    {
        var subject = assertions.Subject;
        var result = subject.Should().ContainSubtree( expectation, because, becauseArgs );

        return result;
    }

    private static JToken ParseWithDatesAsString( this string json )
    {
        using var reader = new JsonTextReader( new StringReader( json ) );
        reader.DateParseHandling = DateParseHandling.None;

        var token = JToken.ReadFrom( reader );

        return token;
    }
}