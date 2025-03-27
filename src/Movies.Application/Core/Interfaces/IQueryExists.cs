﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Core.Interfaces;

/// <summary>
/// Interfaces representing the countable functions perform to the database
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public interface IQueryExists<TEntity> {
  /// <summary>
  /// Verifies existence of record against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the total counts or entities satisfied by the condition</returns>
  public Task<bool> ExistsAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

  /// <summary>
  /// Verifies existence of record against the given query
  /// </summary>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the total counts or entities satisfied by the query</returns>
  public Task<bool> ExistsAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default);
}