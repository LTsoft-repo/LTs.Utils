using LTs.Json.Extensions;
using LTs.Stores.Abstractions;
using LTs.Stores.test.Infrastructure;
using LTs.TestUtils.FluentAssertions;

namespace LTs.Stores.test;

[ Collection( "StoreFileIo" ) ]
public class JsonFileStoreTests : BaseTest
{
    public JsonFileStoreTests( ITestOutputHelper testOutput )
        : base( testOutput ) { }

    #region ReadAsync
    [ Fact ]
    public async Task ReadAsync_WhenEntityMissing_ReturnsNull()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        // Act
        var loaded = await store.ReadAsync( "missing", CancellationToken.None );

        // Assert
        loaded.Should().BeNull();
    }

    [ Fact ]
    public async Task ReadAsync_WhenEntityExists_ReturnsPersistedEntity()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        await store.WriteAsync(
            "sample",
            new JsonFileStoreSampleEntity
            {
                Value = "persisted"
            },
            CancellationToken.None );

        // Act
        var loaded = await store.ReadAsync( "sample", CancellationToken.None );

        // Assert
        loaded.Should().BeEquivalentTo( new JsonFileStoreSampleEntity
        {
            Value = "persisted"
        } );
    }
    #endregion

    #region WriteAsync
    [ Fact ]
    public async Task WriteAsync_CreatesAndReplacesEntity()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        // Act
        await store.WriteAsync(
            "sample",
            new JsonFileStoreSampleEntity
            {
                Value = "initial"
            },
            CancellationToken.None );

        await store.WriteAsync(
            "sample",
            new JsonFileStoreSampleEntity
            {
                Value = "updated"
            },
            CancellationToken.None );

        var loaded = await store.ReadAsync( "sample", CancellationToken.None );

        // Assert
        loaded.Should().BeEquivalentTo( new JsonFileStoreSampleEntity
        {
            Value = "updated"
        } );
    }
    #endregion

    #region ListAsync
    [ Fact ]
    public async Task ListAsync_ReturnsAllPersistedEntities()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        await store.WriteAsync(
            "first",
            new JsonFileStoreSampleEntity
            {
                Value = "one"
            },
            CancellationToken.None );

        await store.WriteAsync(
            "second",
            new JsonFileStoreSampleEntity
            {
                Value = "two"
            },
            CancellationToken.None );

        // Act
        var entities = await store.ListAsync( CancellationToken.None );

        // Assert
        entities.Should()
                .ContainExactlyEquivalent(
                [
                    new JsonFileStoreSampleEntity
                    {
                        Value = "one"
                    },
                    new JsonFileStoreSampleEntity
                    {
                        Value = "two"
                    }
                ] );
    }

    [ Fact ]
    public async Task ListAsync_WhenPathProviderIsNotListable_ThrowsNotSupportedException()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new NonListableStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        // Act
        var act = async () => await store.ListAsync( CancellationToken.None );

        // Assert
        await act.Should().ThrowAsync<NotSupportedException>();
    }
    #endregion

    #region DeleteAsync
    [ Fact ]
    public async Task DeleteAsync_WhenEntityMissing_DoesNotThrow()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        // Act
        var act = async () => await store.DeleteAsync( "missing", CancellationToken.None );

        // Assert
        await act.Should().NotThrowAsync();
    }

    [ Fact ]
    public async Task DeleteAsync_WhenStorePathHasNoParentDirectory_RemovesFile()
    {
        // Arrange
        var workDir = TestProjectTempStorage.CreateUniqueDirectory();
        const string fileName = "bare-entity.json";
        var previousCurrentDirectory = Directory.GetCurrentDirectory();

        try
        {
            Directory.SetCurrentDirectory( workDir );

            var json = new JsonFileStoreSampleEntity
            {
                Value = "bare"
            }.ToJson();

            await File.WriteAllTextAsync( fileName, json );

            IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
                new RelativeFileNameStorePathProvider<JsonFileStoreSampleEntity>( fileName ) );

            // Act
            await store.DeleteAsync( "ignored", CancellationToken.None );

            // Assert
            File.Exists( Path.Combine( workDir, fileName ) ).Should().BeFalse();

            var loaded = await store.ReadAsync( "ignored", CancellationToken.None );
            loaded.Should().BeNull();
        }
        finally
        {
            Directory.SetCurrentDirectory( previousCurrentDirectory );
        }
    }

    [ Fact ]
    public async Task DeleteAsync_WhenOtherEntitiesRemain_RemovesEntityAndKeepsDirectory()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>(
            new FixedStorePathProvider<JsonFileStoreSampleEntity>( storageRoot ) );

        await store.WriteAsync(
            "keep",
            new JsonFileStoreSampleEntity
            {
                Value = "remaining"
            },
            CancellationToken.None );

        await store.WriteAsync(
            "remove",
            new JsonFileStoreSampleEntity
            {
                Value = "deleted"
            },
            CancellationToken.None );

        // Act
        await store.DeleteAsync( "remove", CancellationToken.None );

        // Assert
        Directory.Exists( storageRoot ).Should().BeTrue();
        File.Exists( Path.Combine( storageRoot, "keep.json" ) ).Should().BeTrue();
        File.Exists( Path.Combine( storageRoot, "remove.json" ) ).Should().BeFalse();

        var remaining = await store.ReadAsync( "keep", CancellationToken.None );
        remaining.Should().BeEquivalentTo( new JsonFileStoreSampleEntity
        {
            Value = "remaining"
        } );

        var deleted = await store.ReadAsync( "remove", CancellationToken.None );
        deleted.Should().BeNull();
    }

    [ Fact ]
    public async Task DeleteAsync_RemovesEntityAndEmptyHashDirectory()
    {
        // Arrange
        var storageRoot = TestProjectTempStorage.CreateUniqueDirectory();
        var pathProvider = new HashedStorePathProvider<JsonFileStoreSampleEntity>(
            storageRoot,
            "entity.json",
            new RecordingStorageKeyNormalizer() );

        IStore<JsonFileStoreSampleEntity> store = new JsonFileStore<JsonFileStoreSampleEntity>( pathProvider );
        const string storageKey = "delete-me";

        await store.WriteAsync(
            storageKey,
            new JsonFileStoreSampleEntity
            {
                Value = "stored"
            },
            CancellationToken.None );

        var path = pathProvider.GetPath( storageKey );
        var hashDirectory = Path.GetDirectoryName( path );

        // Act
        await store.DeleteAsync( storageKey, CancellationToken.None );

        // Assert
        File.Exists( path ).Should().BeFalse();
        Directory.Exists( hashDirectory! ).Should().BeFalse();

        var loaded = await store.ReadAsync( storageKey, CancellationToken.None );
        loaded.Should().BeNull();
    }
    #endregion
}