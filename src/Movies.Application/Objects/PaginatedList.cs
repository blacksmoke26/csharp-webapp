// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page#add-paging-to-students-index

using Movies.Contracts.Responses;

namespace Movies.Application.Objects;

public record PaginatorOptions {
  /// <summary>A page number to start skipping from</summary>
  public int Page { get; set; } = 1;

  /// <summary>No. of entities to be fetched</summary>
  public int PageSize { get; set; } = 15;
}

/// <summary>
/// This class represents the fetching and implementation of entities pagination
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedList<T> : List<T> {
  /// <summary>The page number</summary>
  public int PageIndex { get; init; }
  /// <summary>No. of total pages</summary>
  public int TotalPages { get; init; }
  /// <summary>No. of total count</summary>
  public int TotalCount { get; init; }

  /// <summary>Check that next page is available of the current page</summary>
  public bool HasPreviousPage => PageIndex > 1;

  /// <summary>Check that previous page is available of the current page</summary>
  public bool HasNextPage => PageIndex < TotalPages;

  private PaginatedList(List<T> items, int count, int pageIndex, int pageSize) {
    TotalCount = count;
    PageIndex = pageIndex;
    TotalPages = (int)Math.Ceiling(count / (double)pageSize);

    AddRange(items);
  }

  /// <summary>
  /// Transforms the paginated instance into a paginated results 
  /// </summary>
  /// <returns>The paginated result object</returns>
  public PaginatedResult ToPaginatedResult() => new () {
    CurrentPage = PageIndex,
    TotalPages = TotalPages,
    TotalCount = TotalCount, 
    HasPreviousPage = HasPreviousPage,
    HasNextPage = HasNextPage,
    Rows = this.Cast<object>().ToList()
  };

  /// <summary>
  /// Creates the apply pagination to the given sequence
  /// </summary>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="options">The pagination options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The paginated results</returns>
  public static Task<PaginatedList<T>> CreateAsync(
    IQueryable<T> source, PaginatorOptions? options = null, CancellationToken token = default) {
    return CreateAsync<T>(source, options, token);
  }

  /// <summary>
  /// Creates the apply pagination to the given sequence and fetch customized entitles result
  /// </summary>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="options">The pagination options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The paginated results</returns>
  public static Task<PaginatedList<TResult>> CreateAsync<TResult>(
    IQueryable<T> source, PaginatorOptions? options = null, CancellationToken token = default) {
    return CreateWithSelectorAsync<TResult>(source, null, options, token);
  }

  /// <summary>
  /// Creates the apply pagination to the given sequence and fetch customized entitles result
  /// </summary>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="selector">The callback function to fetch the attributes/columns</param>
  /// <param name="options">The pagination options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The paginated results</returns>
  public static async Task<PaginatedList<TResult>> CreateWithSelectorAsync<TResult>(
    IQueryable<T> source, Expression<Func<T, TResult>>? selector, PaginatorOptions? options = null,
    CancellationToken token = default) {
    var count = await source.CountAsync(token);

    var pageIndex = options?.Page ?? 1;
    var pageSize = options?.PageSize ?? 15;

    var items = await (
        selector != null
          ? source.Select(selector).Cast<TResult>()
          : source.Cast<TResult>()
      )
      .Skip((pageIndex - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync(token);

    return new PaginatedList<TResult>(items, count, pageIndex, pageSize);
  }
}