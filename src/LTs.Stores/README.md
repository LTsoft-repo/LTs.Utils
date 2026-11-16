# LTs.Stores

## Introduction
Set of classes and extensions for storing and retrieving entities.

This library includes:
- LTs.Stores.Abstractions
  - IStore&lt;TEntity&gt;
  - IStorePathProvider&lt;TEntity&gt;
  - IListableStorePathProvider&lt;TEntity&gt;
  - IStorageKeyNormalizer
- LTs.Stores
  - JsonFileStore&lt;TEntity&gt;
  - HashedStorePathProvider&lt;TEntity&gt;
- LTs.Stores.DependencyInjection
  - RegistrationExtensions.AddJsonFileStore&lt;TEntity, TPathProvider&gt; ()