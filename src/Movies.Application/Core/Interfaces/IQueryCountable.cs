// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the countable functions perform to the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryCountable<TEntity> {
  /// <summary>
  /// Counts the records against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the total counts or entities satisfied by the condition</returns>
  public Task<int> CountAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

  /// <summary>
  /// Counts the records against the given query
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the total counts or entities satisfied by the query</returns>
  public Task<int> CountAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default);
}