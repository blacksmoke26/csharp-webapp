// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the countable functions perform to the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryPaginated<TEntity> {
  /// <summary>
  /// Fetches the multiple paginated entities
  /// </summary>
  /// <param name="queryable">A callback function to perform a query on a current sequence.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="options">The pagination options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  Task<PaginatedList<TEntity>> GetPaginatedAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TEntity>>? selector,
    PaginatorOptions options, CancellationToken token = default);
  
  /// <summary>
  /// Fetches the multiple paginated custom entities
  /// </summary>
  /// <param name="queryable">A callback function to perform a query on a current sequence.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="token">The cancellation token</param>
  /// <param name="options">The pagination options</param>
  /// <returns>The fetched records</returns>
  Task<PaginatedList<TResult>> GetPaginatedAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TResult>>? selector,
    PaginatorOptions options, CancellationToken token = default);
}