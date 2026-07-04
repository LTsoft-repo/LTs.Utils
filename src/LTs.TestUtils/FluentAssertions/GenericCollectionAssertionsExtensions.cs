using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace LTs.TestUtils.FluentAssertions;

/// <summary>
///     Extensions for <see cref="GenericCollectionAssertions{TCollection,TElement,TAssertions}" />.
/// </summary>
public static class GenericCollectionAssertionsExtensions
{
    private record FirstEquivalentResult<T>
    {
        public bool WasFound { get; init; }
        public T? Item { get; init; }
    }

    private record AssertionOperationResult<TSubject, TFailedExpectation>
    {
        public bool Success { get; init; }

        public TSubject? Subject { get; init; }
        public TFailedExpectation? FailedExpectation { get; init; }
    }

    #region ContainEquivalentSubset (generic)
    /// <summary>
    ///     Asserts that the collection contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined using the default
    ///         global equivalency options configured in <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expectation" /> is <see langword="null" />.</exception>
    public static AndConstraint<TAssertions> ContainEquivalentSubset<TCollection, T, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
        => assertions.ContainEquivalentSubset( expectation, options => options, because, becauseArgs );

    /// <summary>
    ///     Asserts that the collection contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined by comparing the object
    ///         graphs of the elements in both collections, as configured by the <paramref name="config" /> parameter.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" /> or overridden using the <paramref name="config" /> parameter.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="config">
    ///     A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}" /> configuration object used
    ///     to customize the way the object graphs are compared. You can provide a custom configuration to override
    ///     the global defaults determined by <see cref="AssertionOptions" />.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="config" /> or <paramref name="expectation" /> is
    ///     <see langword="null" />.
    /// </exception>
    public static AndConstraint<TAssertions> ContainEquivalentSubset<TCollection, T, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
    {
        ArgumentNullException.ThrowIfNull( config );
        ArgumentNullException.ThrowIfNull( expectation );

        var expectationArray = expectation.ToArray();

        if( !expectationArray.Any() )
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .FailWith( "Cannot assert a subset of an empty collection." );
        }

        // Ensure subject is not null
        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( assertions.Subject is not null )
               .FailWith( "Expected {context:collection} to contain equivalent of {0}{reason}, but found <null>.", expectation );

        // Clone default equivalency options and apply the configuration
        var options = config( AssertionOptions.CloneDefaults<TExpectation>() );

        var scope = new AssertionScope();
        scope.AddReportable( "configuration", options.ToString );

        var assertionResult = assertions.Subject!.ValidateValues( expectationArray, config );

        if( !assertionResult.Success )
        {
            scope.FailWith( "Expected {context:collection} {0} to contain an equivalent of {1}{reason}, but no match was found.",
                            assertionResult.Subject,
                            assertionResult.FailedExpectation );
        }

        scope.Dispose();

        // Return success with a default item since this method is about checking presence
        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }
    #endregion

    #region ContainEquivalentSubset (IGrouping)
    /// <summary>
    ///     Asserts that the collection of IGrouping contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent key and item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined using the default
    ///         global equivalency options configured in <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="TKey">The type of key in the collection being asserted against.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expectation" /> is <see langword="null" />.</exception>
    public static AndConstraint<TAssertions> ContainEquivalentSubset<TCollection, TKey, TElement, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, IGrouping<TKey, TElement>, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<IGrouping<TKey, TElement>>
        where TAssertions : GenericCollectionAssertions<TCollection, IGrouping<TKey, TElement>, TAssertions>
        where TExpectation : IGrouping<TKey, TElement>
        => assertions.ContainEquivalentSubset( expectation, options => options, options => options, because, becauseArgs );

    /// <summary>
    ///     Asserts that the collection contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent key and item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined by comparing the object
    ///         graphs of the elements in both collections, as configured by the <paramref name="configKey" /> and
    ///         <paramref name="configElement" /> parameters.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" /> or overridden using the <paramref name="configKey" /> and
    ///         <paramref name="configElement" /> parameters.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="configKey">
    ///     A reference to the <see cref="EquivalencyAssertionOptions{TKey}" /> configuration object used
    ///     to customize the way the key object graphs are compared. You can provide a custom configuration to override
    ///     the global defaults determined by <see cref="AssertionOptions" />.
    /// </param>
    /// <param name="configElement">
    ///     A reference to the <see cref="EquivalencyAssertionOptions{TElement}" /> configuration object used
    ///     to customize the way the element object graphs are compared. You can provide a custom configuration to override
    ///     the global defaults determined by <see cref="AssertionOptions" />.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="TKey">The type of key in the collection being asserted against.</typeparam>
    /// <typeparam name="TElement">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="configKey" /> or <paramref name="configElement" /> or <paramref name="expectation" /> is
    ///     <see langword="null" />.
    /// </exception>
    public static AndConstraint<TAssertions> ContainEquivalentSubset<TCollection, TKey, TElement, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, IGrouping<TKey, TElement>, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TKey>, EquivalencyAssertionOptions<TKey>> configKey,
        Func<EquivalencyAssertionOptions<TElement>, EquivalencyAssertionOptions<TElement>> configElement,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<IGrouping<TKey, TElement>>
        where TAssertions : GenericCollectionAssertions<TCollection, IGrouping<TKey, TElement>, TAssertions>
        where TExpectation : IGrouping<TKey, TElement>
    {
        ArgumentNullException.ThrowIfNull( configKey );
        ArgumentNullException.ThrowIfNull( configElement );
        ArgumentNullException.ThrowIfNull( expectation );

        var expectationArray = expectation.ToArray();

        if( !expectationArray.Any() )
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .FailWith( "Cannot assert a subset of an empty collection." );
        }

        // Ensure subject is not null
        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( assertions.Subject is not null )
               .FailWith( "Expected {context:collection} to contain equivalent of {0}{reason}, but found <null>.", expectation );

        // Ensure all expected keys are present in the subject
        assertions.Subject!.ValidateGroupingKeys( expectationArray, configKey );

        // Validate the values of each Key
        assertions.Subject!.ValidateGroupingValues( expectationArray, configKey, configElement );

        // Return success with a default item since this method is about checking presence
        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }
    #endregion

    #region NotContainEquivalentInSubset (generic)
    /// <summary>
    ///     Asserts that the collection does not contain any subset of elements equivalent to the items in
    ///     <paramref name="expectation" />, using the default equivalency options.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that none of the items in <paramref name="expectation" /> have an equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined using the default
    ///         global equivalency options configured in <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" />.
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of elements to verify against. Each element is checked to ensure it does not have an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndConstraint{TAssertions}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expectation" /> is <see langword="null" />.</exception>
    public static AndConstraint<TAssertions> NotContainEquivalentInSubset<TCollection, T, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
        => assertions.NotContainEquivalentInSubset( expectation, options => options, because, becauseArgs );

    /// <summary>
    ///     Asserts that the collection does not contain any subset of elements equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that none of the items in <paramref name="expectation" /> have an equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined by comparing the object
    ///         graphs of the elements in both collections, as configured by the <paramref name="config" /> parameter.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" /> or overridden using the <paramref name="config" /> parameter.
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of elements to verify against. Each element is checked to ensure it does not have an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="config">
    ///     A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}" /> configuration object used
    ///     to customize the way the object graphs are compared. You can provide a custom configuration to override
    ///     the global defaults determined by <see cref="AssertionOptions" />.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <typeparam name="TExpectation">The type of elements in the expected collection.</typeparam>
    /// <returns>
    ///     An <see cref="AndConstraint{TAssertions}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="config" /> or <paramref name="expectation" /> is
    ///     <see langword="null" />.
    /// </exception>
    public static AndConstraint<TAssertions> NotContainEquivalentInSubset<TCollection, T, TAssertions, TExpectation>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
    {
        ArgumentNullException.ThrowIfNull( config );
        ArgumentNullException.ThrowIfNull( expectation );

        // Ensure subject is not null
        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( assertions.Subject is not null )
               .FailWith( "Expected {context:collection} to not contain equivalent of {0}{reason}, but found <null>.", expectation );

        // Clone default equivalency options and apply the configuration
        var options = config( AssertionOptions.CloneDefaults<TExpectation>() );

        var scope = new AssertionScope();
        scope.AddReportable( "configuration", options.ToString );

        foreach( var expectedItem in expectation )
        {
            var matchFound = false;

            foreach( var actualItem in assertions.Subject! )
            {
                var context =
                    new EquivalencyValidationContext( Node.From<TExpectation>( () => AssertionScope.Current.CallerIdentity ), options )
                    {
                        Reason = new Reason( because, becauseArgs ),
                        TraceWriter = options.TraceWriter
                    };

                var comparands = new Comparands
                {
                    Subject = actualItem,
                    Expectation = expectedItem,
                    CompileTimeType = typeof( TExpectation )
                };

                new EquivalencyValidator().AssertEquality( comparands, context );

                var failures = scope.Discard();

                if( !failures.Any() )
                {
                    matchFound = true;

                    break;
                }
            }

            // If match was found for an expected item, fail
            if( matchFound )
            {
                Execute.Assertion
                       .BecauseOf( because, becauseArgs )
                       .FailWith( "Expected {context:collection} to not contain equivalent of {0}{reason}, but a match was found.", expectedItem );

                break;
            }
        }

        scope.Dispose();

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }
    #endregion

    #region ContainExactlyEquivalent
    /// <summary>
    ///     Asserts that the collection contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined using the default
    ///         global equivalency options configured in <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expectation" /> is <see langword="null" />.</exception>
    public static AndConstraint<TAssertions> ContainExactlyEquivalent<TCollection, T, TAssertions>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<T> expectation,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
        => assertions.ContainExactlyEquivalent( expectation, opts => opts, because, becauseArgs );

    /// <summary>
    ///     Asserts that the collection contains a subset of elements that are equivalent to the items in
    ///     <paramref name="expectation" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method verifies that each item in <paramref name="expectation" /> has at least one equivalent item
    ///         in the <paramref name="assertions.Subject" /> collection. Equivalency is determined using the default
    ///         global equivalency options configured in <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         By default, objects are considered equivalent when their properties have the same names and values,
    ///         regardless of their runtime types. The actual behavior can be influenced by global defaults managed by
    ///         <see cref="AssertionOptions" />.
    ///     </para>
    ///     <para>
    ///         Note: This method ensures that all items in <paramref name="expectation" /> are matched but does not require
    ///         all items in the collection to be matched (i.e., it allows extra items in the collection).
    ///     </para>
    /// </remarks>
    /// <param name="assertions">The collection assertions.</param>
    /// <param name="expectation">
    ///     The collection of expected elements to verify. Each element is checked to ensure it has an equivalent
    ///     counterpart in the <paramref name="assertions.Subject" /> collection.
    /// </param>
    /// <param name="config">
    ///     A reference to the <see cref="EquivalencyAssertionOptions{T}" /> configuration object used
    ///     to customize the way the object graphs are compared. You can provide a custom configuration to override
    ///     the global defaults determined by <see cref="AssertionOptions" />.
    /// </param>
    /// <param name="because">
    ///     A formatted phrase explaining why the assertion is needed, as supported by
    ///     <see cref="string.Format(string,object[])" />. If the phrase does not start with the word "because," it is
    ///     automatically prepended.
    /// </param>
    /// <param name="becauseArgs">
    ///     Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <typeparam name="TCollection">The type of the collection being asserted against.</typeparam>
    /// <typeparam name="T">The type of elements in the collection being asserted against.</typeparam>
    /// <typeparam name="TAssertions">The type of the collection assertions.</typeparam>
    /// <returns>
    ///     An <see cref="AndWhichConstraint{TAssertions,T}" /> for further chaining of assertions.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="expectation" /> is <see langword="null" />.</exception>
    public static AndConstraint<TAssertions> ContainExactlyEquivalent<TCollection, T, TAssertions>(
        this GenericCollectionAssertions<TCollection, T, TAssertions> assertions,
        IEnumerable<T> expectation,
        Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config,
        string because = "",
        params object[] becauseArgs )
        where TCollection : IEnumerable<T>
        where TAssertions : GenericCollectionAssertions<TCollection, T, TAssertions>
    {
        ArgumentNullException.ThrowIfNull( expectation );
        ArgumentNullException.ThrowIfNull( config );

        // collect arrays
        var expectedArray = expectation.ToArray();

        Execute.Assertion
               .BecauseOf( because, becauseArgs )
               .ForCondition( assertions.Subject is not null )
               .FailWith( "Expected {context:collection} to be equivalent to {0}{reason}, but found <null>.", expectedArray );

        var subjectArray = assertions.Subject!.ToArray();

        // special case: expecting empty
        if( !expectedArray.Any() )
        {
            Execute.Assertion
                   .BecauseOf( because, becauseArgs )
                   .ForCondition( !subjectArray.Any() )
                   .FailWith( "Expected {context:collection} to be empty{reason}, but found {0}.", subjectArray );

            return new AndConstraint<TAssertions>( (TAssertions)assertions );
        }

        // clone options
        var options = config( AssertionOptions.CloneDefaults<T>() );

        using var scope = new AssertionScope();
        scope.AddReportable( "configuration", options.ToString );

        // 1) every expected has a match in subject
        var missingInSubject = subjectArray.ValidateValues( expectedArray, _ => options );

        if( !missingInSubject.Success )
        {
            scope.FailWith(
                "Expected {context:collection} {0} to contain an equivalent of {1}{reason}, but no match was found.",
                missingInSubject.Subject,
                missingInSubject.FailedExpectation );
        }

        // 2) every actual has a match in expected
        var extrasInSubject = expectedArray.ValidateValues( subjectArray, _ => options );

        if( !extrasInSubject.Success )
        {
            scope.FailWith(
                "Expected {context:collection} to not contain extra items, but found an unexpected {1}{reason} in {0}.",
                extrasInSubject.Subject,
                extrasInSubject.FailedExpectation );
        }

        return new AndConstraint<TAssertions>( (TAssertions)assertions );
    }
    #endregion

    /// <summary>
    ///     Validates that the subject collection contains an equivalent of each item in the expectation collection.
    /// </summary>
    /// <typeparam name="TSubject">Type of the subject collection.</typeparam>
    /// <typeparam name="TExpectation">Type of elements in the expectation collection.</typeparam>
    /// <param name="subject">Subject collection to validate.</param>
    /// <param name="expectation">Expectation collection to validate against.</param>
    /// <param name="config">Equivalency assertion options to customize the comparison.</param>
    private static AssertionOperationResult<IEnumerable<TSubject>, TExpectation> ValidateValues<TSubject, TExpectation>(
        this IEnumerable<TSubject> subject,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config )
    {
        ArgumentNullException.ThrowIfNull( subject );

        var subjectArray = subject.ToArray();

        var result = new AssertionOperationResult<IEnumerable<TSubject>, TExpectation>
        {
            Success = true,
            Subject = subjectArray
        };

        foreach( var expectedElement in expectation )
        {
            var findResult = subjectArray.FirstEquivalent( expectedElement, config );

            // ReSharper disable once InvertIf
            if( !findResult.WasFound )
            {
                result = result with
                {
                    Success = false,
                    FailedExpectation = expectedElement
                };

                break;
            }
        }

        return result;
    }

    private static void ValidateGroupingKeys<TKey, TElement, TExpectation>(
        this IEnumerable<IGrouping<TKey, TElement>> subject,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TKey>, EquivalencyAssertionOptions<TKey>> config )
        where TExpectation : IGrouping<TKey, TElement>
    {
        var subjectKeys = subject.Select( x => x.Key ).ToArray();
        var expectationKeys = expectation.Select( x => x.Key ).ToArray();

        var options = config( AssertionOptions.CloneDefaults<TKey>() );
        var scope = new AssertionScope();
        scope.AddReportable( "configuration", options.ToString );

        var assertionResult = subjectKeys.ValidateValues( expectationKeys, config );

        if( !assertionResult.Success )
        {
            scope.FailWith( "Expected Keys {0} to contain equivalent of {1}{reason}, but no match was found.",
                            subjectKeys,
                            assertionResult.FailedExpectation );
        }

        scope.Dispose();
    }

    private static void ValidateGroupingValues<TKey, TElement, TExpectation>(
        this IEnumerable<IGrouping<TKey, TElement>> subject,
        IEnumerable<TExpectation> expectation,
        Func<EquivalencyAssertionOptions<TKey>, EquivalencyAssertionOptions<TKey>> configKey,
        Func<EquivalencyAssertionOptions<TElement>, EquivalencyAssertionOptions<TElement>> configElement )
        where TExpectation : IGrouping<TKey, TElement>
    {
        var expectationArray = ( (IEnumerable<IGrouping<TKey, TElement>>)expectation ).ToArray();

        // ReSharper disable once RedundantCast
        //var subjectArray = ( (IEnumerable<IGrouping<TKey, TElement>>)subject ).ToArray();
        var subjectArray = subject.ToArray();

        var options = configElement( AssertionOptions.CloneDefaults<TElement>() );
        var scope = new AssertionScope();
        scope.AddReportable( "configuration", options.ToString );

        foreach( var expectedGroup in expectationArray )
        {
            var expectedKey = expectedGroup.Key;
            var expectedValues = expectedGroup.ToArray();

            var groupFindResult = subjectArray.FirstEquivalent( expectedKey, configKey );

            if( !groupFindResult.WasFound )
            {
                scope.FailWith( "Expected a group with Key {0} to exist in the subject collection, but it was not found.", expectedKey );

                break;
            }

            var subjectGroup = groupFindResult.Item;
            var subjectValues = subjectGroup?.ToArray() ?? Array.Empty<TElement>();

            var assertionResult = subjectValues.ValidateValues( expectedValues, configElement );

            if( !assertionResult.Success )
            {
                scope.FailWith(
                    "Expected group of Key {0} with {context:collection} {1} to contain an equivalent of {2}{reason}, but no match was found.",
                    expectedKey,
                    assertionResult.Subject,
                    assertionResult.FailedExpectation );

                break;
            }

            var failures = scope.Discard();

            if( failures.Any() )
            {
                break; // An error occurred.
            }
        }

        scope.Dispose();
    }

    private static FirstEquivalentResult<T?> FirstEquivalent<T, TExpectation>(
        this IEnumerable<T> collection,
        TExpectation expectation,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config )
        => collection.FirstEquivalent( expectation, x => x, config );

    private static FirstEquivalentResult<IGrouping<TKey?, TElement>?> FirstEquivalent<TKey, TElement, TExpectation>(
        this IEnumerable<IGrouping<TKey?, TElement>> collection,
        TExpectation key,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config )
        => collection.FirstEquivalent( key, x => x.Key, config );

    private static FirstEquivalentResult<T?> FirstEquivalent<T, TSelector, TExpectation>(
        this IEnumerable<T> collection,
        TExpectation expectation,
        Func<T, TSelector> selector,
        Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config )
    {
        var wasFound = false;

        var item = collection.FirstOrDefault( x =>
            {
                string[] failures;

                using( var internalScope = new AssertionScope() )
                {
                    var options = config( AssertionOptions.CloneDefaults<TExpectation>() );
                    internalScope.AddReportable( "configuration", options.ToString );

                    var subject = selector( x );

                    var act = () => expectation is null
                                        ? subject.Should().BeNull()
                                        : subject.Should().BeEquivalentTo( expectation, config );

                    act.Should().NotThrow();

                    failures = internalScope.Discard();
                }

                wasFound = failures.Length == 0;

                return wasFound;
            } );

        return new FirstEquivalentResult<T?>
        {
            WasFound = wasFound,
            Item = item
        };
    }
}