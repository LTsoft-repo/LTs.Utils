using FluentAssertions;
using FluentAssertions.Execution;
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
    public static AndConstraint<TAssertions> BeSameJsonAs<TAssertions>( this StringAssertions<TAssertions> assertions,
                                                                        string expectation,
                                                                        string because = "",
                                                                        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
        => assertions.BeSameJsonAs( expectation, JsonAssertionOptions.Default, because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="options">The JSON assertion options.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<TAssertions> BeSameJsonAs<TAssertions>( this StringAssertions<TAssertions> assertions,
                                                                        string expectation,
                                                                        JsonAssertionOptions options,
                                                                        string because = "",
                                                                        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
    {
        var subject = assertions.Subject.ParseWithDatesAsString();
        var expected = expectation;

        subject.Should().BeSameJsonAs( expected, options, because, becauseArgs );

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="configureOptions">The JSON assertion options configuration.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<TAssertions> BeSameJsonAs<TAssertions>( this StringAssertions<TAssertions> assertions,
                                                                        string expectation,
                                                                        Func<JsonAssertionOptions, JsonAssertionOptions> configureOptions,
                                                                        string because = "",
                                                                        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
        => assertions.BeSameJsonAs( expectation, configureOptions( JsonAssertionOptions.Default ), because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <typeparam name="TAssertions">The type of the assertions</typeparam>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="excludedJsonPaths">The JSON paths to exclude from the comparison.</param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<TAssertions> BeSameJsonAs<TAssertions>( this StringAssertions<TAssertions> assertions,
                                                                        string expectation,
                                                                        IEnumerable<string> excludedJsonPaths,
                                                                        string because = "",
                                                                        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { ExcludedJsonPaths = excludedJsonPaths },
                                    because,
                                    becauseArgs );

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
        => assertions.BeSameJsonAs( expectation, JsonAssertionOptions.Default, because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="options">The JSON assertion options.</param>
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
        JsonAssertionOptions options,
        string because = "",
        params object[] becauseArgs )
    {
        var stringSubject = assertions.Subject.ToString( Formatting.None );
        var subject = stringSubject.ParseWithDatesAsString();
        var expected = expectation.ParseWithDatesAsString();

        return subject.Should().BeSameJsonAs( expected, options, because, becauseArgs );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="configureOptions">The JSON assertion options configuration.</param>
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
        Func<JsonAssertionOptions, JsonAssertionOptions> configureOptions,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation, configureOptions( JsonAssertionOptions.Default ), because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="excludedJsonPaths">The JSON paths to exclude from the comparison.</param>
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
        IEnumerable<string> excludedJsonPaths,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { ExcludedJsonPaths = excludedJsonPaths },
                                    because,
                                    becauseArgs );

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
        => assertions.BeSameJsonAs( expectation, JsonAssertionOptions.Default, because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="options">The JSON assertion options.</param>
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
        JsonAssertionOptions options,
        string because = "",
        params object[] becauseArgs )
    {
        var excludedPaths = options.ExcludedJsonPaths.Concat( options.Matchers.Select( x => x.JsonPath ) ).ToArray();
        var subject = assertions.Subject.RemoveExcludedJsonPaths( excludedPaths );
        var expected = expectation.RemoveExcludedJsonPaths( excludedPaths );

        var differences = GetJsonDifferences( subject, expected, options )
                          .Concat( GetMatcherDifferences( assertions.Subject, expectation, options.Matchers ) )
                          .ToArray();

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( differences.Length == 0 )
               .FailWith( GetFailureMessage( differences ) + "{reason}" );

        return new AndConstraint<JTokenAssertions>( assertions );
    }

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string using the specified options.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="configureOptions">The JSON assertion options configuration.</param>
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
        Func<JsonAssertionOptions, JsonAssertionOptions> configureOptions,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation, configureOptions( JsonAssertionOptions.Default ), because, becauseArgs );

    /// <summary>
    ///     Asserts that the JSON string is equivalent to the expected JSON string without extra fields.
    /// </summary>
    /// <param name="assertions">The assertions.</param>
    /// <param name="expectation">The expected elements to verify.</param>
    /// <param name="excludedJsonPaths">The JSON paths to exclude from the comparison.</param>
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
        IEnumerable<string> excludedJsonPaths,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { ExcludedJsonPaths = excludedJsonPaths },
                                    because,
                                    becauseArgs );

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
    [ Obsolete( "Use BeSameJsonAs with JsonAssertionOptions.IgnoreExtraFields instead.", false ) ]
    public static AndConstraint<TAssertions> BeSameJsonIgnoringExtraFieldsAs<TAssertions>(
        this StringAssertions<TAssertions> assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
        where TAssertions : StringAssertions<TAssertions>
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { IgnoreExtraFields = true },
                                    because,
                                    becauseArgs );

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
    [ Obsolete( "Use BeSameJsonAs with JsonAssertionOptions.IgnoreExtraFields instead.", false ) ]
    public static AndConstraint<JTokenAssertions> BeSameJsonIgnoringExtraFieldsAs(
        this JTokenAssertions assertions,
        string expectation,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { IgnoreExtraFields = true },
                                    because,
                                    becauseArgs );

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
    [ Obsolete( "Use BeSameJsonAs with JsonAssertionOptions.IgnoreExtraFields instead.", false ) ]
    public static AndConstraint<JTokenAssertions> BeSameJsonIgnoringExtraFieldsAs(
        this JTokenAssertions assertions,
        JToken expectation,
        string because = "",
        params object[] becauseArgs )
        => assertions.BeSameJsonAs( expectation,
                                    new JsonAssertionOptions { IgnoreExtraFields = true },
                                    because,
                                    becauseArgs );

    private static JToken ParseWithDatesAsString( this string json )
    {
        using var reader = new JsonTextReader( new StringReader( json ) );
        reader.DateParseHandling = DateParseHandling.None;

        var token = JToken.ReadFrom( reader );

        return token;
    }

    private static JToken RemoveExcludedJsonPaths( this JToken token, IEnumerable<string> excludedJsonPaths )
    {
        var clone = token.DeepClone();

        foreach( var jsonPath in excludedJsonPaths.Where( x => !string.IsNullOrWhiteSpace( x ) ) )
        {
            foreach( var selectedToken in clone.SelectTokens( NormalizeJsonPath( jsonPath ) ).ToArray() )
            {
                RemoveToken( selectedToken );
            }
        }

        return clone;
    }

    private static string NormalizeJsonPath( string jsonPath )
        => jsonPath.StartsWith( "$", StringComparison.Ordinal )
               ? jsonPath
               : $"$.{jsonPath}";

    private static void RemoveToken( JToken token )
    {
        if( token.Parent is JProperty property )
        {
            property.Remove();

            return;
        }

        token.Remove();
    }

    private static IEnumerable<string> GetJsonDifferences( JToken subject,
                                                           JToken expectation,
                                                           JsonAssertionOptions options,
                                                           string path = "$" )
    {
        if( subject.Type != expectation.Type )
        {
            yield return
                $"JSON document has a different value at {path}. Expected {FormatJsonValue( expectation )}, but found {FormatJsonValue( subject )}.";

            yield break;
        }

        switch( subject )
        {
            case JObject subjectObject when expectation is JObject expectationObject:
                foreach( var difference in GetObjectDifferences( subjectObject, expectationObject, options, path ) )
                {
                    yield return difference;
                }

                break;

            case JArray subjectArray when expectation is JArray expectationArray:
                foreach( var difference in GetArrayDifferences( subjectArray, expectationArray, options, path ) )
                {
                    yield return difference;
                }

                break;

            default:
                if( !JToken.DeepEquals( subject, expectation ) )
                {
                    yield return
                        $"JSON document has a different value at {path}. Expected {FormatJsonValue( expectation )}, but found {FormatJsonValue( subject )}.";
                }

                break;
        }
    }

    private static IEnumerable<string> GetObjectDifferences( JObject subject,
                                                             JObject expectation,
                                                             JsonAssertionOptions options,
                                                             string path )
    {
        if( !options.IgnoreExtraFields )
        {
            foreach( var property in subject.Properties() )
            {
                if( expectation.Property( property.Name ) is null )
                {
                    yield return $"JSON document has extra property {CombineJsonPath( path, property.Name )}.";
                }
            }
        }

        foreach( var expectedProperty in expectation.Properties() )
        {
            var subjectProperty = subject.Property( expectedProperty.Name );
            var propertyPath = CombineJsonPath( path, expectedProperty.Name );

            if( subjectProperty is null )
            {
                yield return $"JSON document misses property {propertyPath}.";

                continue;
            }

            foreach( var difference in GetJsonDifferences( subjectProperty.Value, expectedProperty.Value, options, propertyPath ) )
            {
                yield return difference;
            }
        }
    }

    private static IEnumerable<string> GetArrayDifferences( JArray subject,
                                                            JArray expectation,
                                                            JsonAssertionOptions options,
                                                            string path )
    {
        var commonLength = Math.Min( subject.Count, expectation.Count );

        for( var i = 0; i < commonLength; i++ )
        {
            foreach( var difference in GetJsonDifferences( subject[ i ], expectation[ i ], options, $"{path}[{i}]" ) )
            {
                yield return difference;
            }
        }

        for( var i = commonLength; i < subject.Count; i++ )
        {
            yield return $"JSON document has extra item {path}[{i}].";
        }

        for( var i = commonLength; i < expectation.Count; i++ )
        {
            yield return $"JSON document misses item {path}[{i}].";
        }
    }

    private static string CombineJsonPath( string path, string propertyName )
        => $"{path}.{propertyName}";

    private static string FormatJsonValue( JToken token )
        => token.ToString( Formatting.None );

    private static IEnumerable<string> GetMatcherDifferences( JToken subject,
                                                              JToken expectation,
                                                              IEnumerable<JsonAssertionMatcher> matchers )
    {
        foreach( var matcher in matchers )
        {
            foreach( var difference in GetMatcherDifferences( subject, matcher, "subject" ) )
            {
                yield return difference;
            }

            foreach( var difference in GetMatcherDifferences( expectation, matcher, "expectation" ) )
            {
                yield return difference;
            }
        }
    }

    private static IEnumerable<string> GetMatcherDifferences( JToken token, JsonAssertionMatcher matcher, string documentName )
    {
        var selectedTokens = token.SelectTokens( NormalizeJsonPath( matcher.JsonPath ) ).ToArray();

        if( selectedTokens.Length == 0 )
        {
            yield return $"JSON document {documentName} misses matcher path {NormalizeJsonPath( matcher.JsonPath )}.";

            yield break;
        }

        foreach( var selectedToken in selectedTokens )
        {
            var selectedPath = FormatJsonPath( selectedToken );
            string? matcherFailure = null;

            try
            {
                matcher.Match( FormatMatcherValue( selectedToken ) );
            }
            catch( Exception exception )
            {
                matcherFailure = $"JSON document {documentName} matcher failed at {selectedPath}. {exception.Message}";
            }

            if( matcherFailure is not null )
            {
                yield return matcherFailure;
            }
        }
    }

    private static string FormatJsonPath( JToken token )
    {
        if( string.IsNullOrWhiteSpace( token.Path ) )
        {
            return "$";
        }

        return token.Path.StartsWith( "[", StringComparison.Ordinal )
                   ? $"${token.Path}"
                   : $"$.{token.Path}";
    }

    private static string? FormatMatcherValue( JToken token )
        => token.Type == JTokenType.Null
               ? null
               : token.Type == JTokenType.String
                   ? token.Value<string>()
                   : token.ToString( Formatting.None );

    private static string GetFailureMessage( IReadOnlyCollection<string> differences )
        => differences.Count == 1
               ? differences.Single()
               : $"JSON document has {differences.Count} mismatches:{Environment.NewLine}{string.Join( Environment.NewLine, differences )}";
}