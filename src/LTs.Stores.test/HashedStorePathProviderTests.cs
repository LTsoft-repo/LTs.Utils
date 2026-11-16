using System.Security.Cryptography;
using System.Text;
using LTs.Stores.test.Infrastructure;
using LTs.TestUtils.FluentAssertions;

namespace LTs.Stores.test;

public class HashedStorePathProviderTests : BaseTest
{
    public HashedStorePathProviderTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region GetPath
    [ Fact ]
    public void GetPath_NormalizesKeyAndUsesHashDirectory()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();
        var normalizer = new RecordingStorageKeyNormalizer( static key => key.Trim().ToLowerInvariant() );
        var provider = new HashedStorePathProvider<JsonFileStoreSampleEntity>(
            storageRoot,
            "entity.json",
            normalizer );
        const string storageKey = "  Sample-Key  ";
        const string normalizedKey = "sample-key";
        var expectedHash = Convert.ToHexString( SHA256.HashData( Encoding.UTF8.GetBytes( normalizedKey ) ) );

        // Act
        var path = provider.GetPath( storageKey );

        // Assert
        normalizer.LastStorageKey.Should().Be( storageKey );
        path.Should().Be( Path.Combine( storageRoot, expectedHash, "entity.json" ) );
    }

    [ Fact ]
    public void GetPath_SameNormalizedKey_ReturnsSamePath()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();
        var provider = new HashedStorePathProvider<JsonFileStoreSampleEntity>(
            storageRoot,
            "entity.json",
            new RecordingStorageKeyNormalizer() );

        // Act
        var firstPath = provider.GetPath( "key-a" );
        var secondPath = provider.GetPath( "key-a" );

        // Assert
        secondPath.Should().Be( firstPath );
    }
    #endregion

    #region EnumerateEntityFilePaths
    [ Fact ]
    public void EnumerateEntityFilePaths_WhenRootMissing_ReturnsEmpty()
    {
        // Arrange
        var missingRoot = Path.Combine( Path.GetTempPath(), Guid.NewGuid().ToString( "N" ) );
        var provider = new HashedStorePathProvider<JsonFileStoreSampleEntity>(
            missingRoot,
            "entity.json",
            new RecordingStorageKeyNormalizer() );

        // Act
        var paths = provider.EnumerateEntityFilePaths().ToList();

        // Assert
        paths.Should().BeEmpty();
    }

    [ Fact ]
    public void EnumerateEntityFilePaths_ReturnsEntityFilesUnderRoot()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();
        var provider = new HashedStorePathProvider<JsonFileStoreSampleEntity>(
            storageRoot,
            "entity.json",
            new RecordingStorageKeyNormalizer() );
        var expectedPath = provider.GetPath( "listed" );
        Directory.CreateDirectory( Path.GetDirectoryName( expectedPath )! );
        File.WriteAllText( expectedPath, "{}" );

        // Act
        var paths = provider.EnumerateEntityFilePaths().ToList();

        // Assert
        paths.Should().ContainExactlyEquivalent( [ expectedPath ] );
    }
    #endregion
}