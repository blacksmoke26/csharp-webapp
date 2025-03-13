// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the single record/entity fetch from the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public partial interface IDbRecordFetch<TEntity> {
  /// <summary>
  /// Fetches a single entity against the given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default);

  /// <summary>
  /// Fetches a single entity against the queryable condition
  /// </summary>
  /// <param name="query">The callback which performs against queryable object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken token = default);

  /// <summary>
  /// Fetches a single entity with selected attributes against the queryable condition
  /// </summary>
  /// <param name="query">The callback which performs against queryable object</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    Expression<Func<TEntity, TEntity>>? selector = null, CancellationToken token = default);

  /// <summary>
  /// Fetches a single record with selected attributes against the queryable condition
  /// </summary>
  /// <param name="query">The callback which performs against queryable object</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  Task<TResult?> GetOneAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    Expression<Func<TEntity, TResult>>? selector = null, CancellationToken token = default);
}

/// <summary>
/// Interfaces representing the multiple records/entities fetch from the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public partial interface IDbRecordFetch<TEntity> {
  /// <summary>
  /// Fetches the multiple records against the given condition
  /// </summary>
  /// <param name="where">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? where = null,
    CancellationToken token = default);

  /// <summary>
  /// Fetches the multiple records against the given condition
  /// </summary>
  /// <param name="query">The callback which performs against queryable object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  Task<List<TEntity>> GetManyAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken token = default);

  /// <summary>
  /// Fetches a multiple record with selected attributes against the queryable condition
  /// </summary>
  /// <param name="query">The callback which performs against queryable object</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched partial records</returns>
  public Task<List<TResult>> GetManyAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? query,
    Expression<Func<TEntity, TResult>>? selector = null,
    CancellationToken token = default);
}

/// <summary>
/// Interfaces representing the generic functions perform to the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public partial interface IDbRecordFetch<TEntity> {
  /// <summary>
  /// Checks the records exists against the given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True upon exist, false otherwise</returns>
  public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default);

  /// <summary>
  /// Deletes one or many records against then given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the count of deleted records</returns>
  public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default);
}