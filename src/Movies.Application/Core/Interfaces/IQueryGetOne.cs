// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the single record/entity fetch from the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryGetOne<TEntity> {
  /// <summary>
  /// Fetches a single entity against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

  /// <summary>
  /// Fetches a single entity against the queryable condition
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default);

  /// <summary>
  /// Fetches a single entity with selected attributes against the queryable condition
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable,
    Expression<Func<TEntity, TEntity>>? selector = null, CancellationToken token = default);

  /// <summary>
  /// Fetches a single record with selected attributes against the queryable condition
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TResult?> GetOneAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable,
    Expression<Func<TEntity, TResult>>? selector = null, CancellationToken token = default);
}