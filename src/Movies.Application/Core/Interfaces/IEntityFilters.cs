// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Contracts.Base;

namespace Movies.Application.Core.Interfaces;

public interface IEntityFilters<TEntity, in TFetchQuery> where TFetchQuery : RequestQueryFetching {
  /// <summary>List of sort by fields supported by the query</summary>
  public static abstract IEnumerable<string> SortByFields { get; }

  /// <summary>
  /// Get the queryable sequence which contains conditions to filter entities and sorting orders
  /// </summary>
  /// <param name="query">The request query DTO object</param>
  /// <param name="userId">The authenticated user ID used to filter unavailable entities</param>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static abstract Func<IQueryable<TEntity>, IQueryable<TEntity>> GetAllQuery(TFetchQuery query,
    long? userId = null);

  /// <summary>
  /// Sorts the elements of a sequence in ascending or descending order according to a request query fields.
  /// </summary>
  /// <param name="query">The request query DTO object</param>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static abstract Func<IQueryable<TEntity>, IQueryable<TEntity>> GetAllSortBy(TFetchQuery query);
}