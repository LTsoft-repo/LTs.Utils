using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace LTs.Configurations.Extensions;

/// <summary>
///     Extensions to parse the tag <c>&lt;EmptyString&gt;</c> as an empty string.
/// </summary>
public static class ConfigurationBuilderEmptyStringExtensions
{
    /// <summary>
    ///     Parse the tag <c>&lt;EmptyString&gt;</c> as an empty string.
    /// </summary>
    /// <param name="configurationBuilder">Configuration builder to use.</param>
    /// <param name="registerSource">Action to register the source.</param>
    /// <returns>The configuration builder.</returns>
    public static IConfigurationBuilder ParseEmptyString(
        this IConfigurationBuilder configurationBuilder,
        Action<IConfigurationBuilder> registerSource )
    {
        registerSource( new WrappedConfigurationBuilder( configurationBuilder ) );

        return configurationBuilder;
    }

    /// <summary>
    ///     Wrapper that parses the tag <c>&lt;EmptyString&gt;</c> as an empty string.
    /// </summary>
    private class WrappedConfigurationBuilder : IConfigurationBuilder
    {
        private readonly IConfigurationBuilder baseBuilder;

        /// <summary>
        ///     Creates a new instance of <see cref="WrappedConfigurationBuilder" />.
        /// </summary>
        /// <param name="baseBuilder">Base configuration builder.</param>
        public WrappedConfigurationBuilder( IConfigurationBuilder baseBuilder )
            => this.baseBuilder = baseBuilder;

        public IConfigurationBuilder Add( IConfigurationSource source )
            => baseBuilder.Add( new WrappedConfigurationSource( source ) );

        public IConfigurationRoot Build()
            => baseBuilder.Build();

        public IDictionary<string, object> Properties => baseBuilder.Properties;
        public IList<IConfigurationSource> Sources => baseBuilder.Sources;
    }

    /// <summary>
    ///     Wrapper that parses the tag <c>&lt;EmptyString&gt;</c> as an empty string.
    /// </summary>
    private class WrappedConfigurationSource : IConfigurationSource
    {
        private readonly IConfigurationSource baseSource;

        /// <summary>
        ///     Creates a new instance of <see cref="WrappedConfigurationSource" />.
        /// </summary>
        /// <param name="baseSource"></param>
        public WrappedConfigurationSource( IConfigurationSource baseSource )
            => this.baseSource = baseSource;

        public IConfigurationProvider Build( IConfigurationBuilder builder ) =>
            new CleanEmptyStringConfigurationProvider( baseSource.Build( builder ) );
    }

    /// <summary>
    ///     Configuration provider that parses the tag <c>&lt;EmptyString&gt;</c> as an empty string.
    /// </summary>
    private class CleanEmptyStringConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfigurationProvider baseProvider;

        /// <summary>
        ///     Creates a new instance of <see cref="CleanEmptyStringConfigurationProvider" />.
        /// </summary>
        /// <param name="baseProvider"></param>
        public CleanEmptyStringConfigurationProvider( IConfigurationProvider baseProvider )
            => this.baseProvider = baseProvider;

        public bool TryGet( string key, out string value )
        {
            value = string.Empty;

            if( baseProvider.TryGet( key, out var nullableValue ) )
            {
                nullableValue ??= string.Empty;
                value = nullableValue.Replace( "<EmptyString>", string.Empty );

                return true;
            }

            return false;
        }

        public void Set( string key, string? value )
            => baseProvider.Set( key, value );

        public IChangeToken GetReloadToken() => baseProvider.GetReloadToken();

        public void Load()
            => baseProvider.Load();

        public IEnumerable<string> GetChildKeys( IEnumerable<string> earlierKeys, string? parentPath ) =>
            baseProvider.GetChildKeys( earlierKeys, parentPath );
    }
}