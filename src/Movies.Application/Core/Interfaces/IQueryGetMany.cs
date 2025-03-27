// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the multiple records/entities fetch from the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryGetMany<TEntity> {
  /// <summary>
  /// Fetches the multiple records against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? predicate = null,
    CancellationToken token = default);

  /// <summary>
  /// Fetches the multiple records against the given condition
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  Task<List<TEntity>> GetManyAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default);

  /// <summary>
  /// Fetches a multiple record with selected attributes against the queryable condition
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched partial records</returns>
  public Task<List<TResult>> GetManyAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TResult>>? selector = null,
    CancellationToken token = default);
}