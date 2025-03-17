// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the deletion functions perform to the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryDeletable<TEntity> {
  /// <summary>
  /// Deletes one or many records against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True upon exist, false otherwise</returns>
  public Task<int> DeleteAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

  /// <summary>
  /// Deletes one or many records against the given queryable sequence
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the count of deleted records</returns>
  public Task<int> DeleteAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default);
}