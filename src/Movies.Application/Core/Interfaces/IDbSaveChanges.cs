// Licensed to the end entitys under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Core.Interfaces;

public interface IDbSaveChanges<TEntity> {
  /// <summary>
  /// Writes the entity object changes into a database
  /// </summary>
  /// <param name="entity">The changed entity object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The entity instance if updated successfully, Null upon failed</returns>
  public Task<TEntity?> SaveAsync(TEntity entity, CancellationToken token = default);
}