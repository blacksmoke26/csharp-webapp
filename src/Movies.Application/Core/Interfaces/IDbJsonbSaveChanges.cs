// Licensed to the end entitys under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

public interface IDbJsonbSaveChanges<TEntity> {
  /// <summary>
  /// Writes the entity object changes with JSONB into a database
  /// </summary>
  /// <param name="entity">The changed entity object</param>
  /// <param name="jsonbChanged">When you make changes in JSONB columns, set this true to save changes as well</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The entity instance if updated successfully, Null upon failed</returns>
  public Task<TEntity?> SaveAsync(
    TEntity entity, bool jsonbChanged = true, CancellationToken token = default);
}