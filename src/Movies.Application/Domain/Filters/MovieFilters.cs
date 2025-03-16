// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Core.Extensions;
using Movies.Contracts.Requests.Query;

namespace Movies.Application.Domain.Filters;

public static class MovieFilters {
  public const string SortByTitle = "title";
  public const string SortByYear = "year";

  public static short YearMin => 1900;
  public static short YearMax => (short)DateTime.UtcNow.Year;
  
  /// <summary>
  /// List of status only applicable for owner/creator users
  /// </summary>
  public static MovieStatus[] CreatorStatuses => [
    MovieStatus.Draft, MovieStatus.Pending, MovieStatus.Published
  ];

  /// <summary>
  /// List of status only applicable for end users / anonymous users
  /// </summary>
  public static MovieStatus[] ReaderStatuses => [
    MovieStatus.Published
  ];

  /// <summary>
  /// Filters the records against reader or creator statuses
  /// </summary>
  /// <param name="userId">The user ID</param>
  /// <returns>A function to test each element for a condition.</returns>
  public static Expression<Func<Movie, bool>> StatusPermissionsFilter(long? userId)
    => x => x.UserId == userId
      ? CreatorStatuses.Contains(x.Status)
      : ReaderStatuses.Contains(x.Status);

  public static Func<IQueryable<Movie>, IQueryable<Movie>> GetAllFilters(
    MoviesGetAllQuery query, long? userId = null) {
    return q => q
      .FilterILike("Title", query.Title)
      .FilterCompare("UserId", query.UserId, FilterComparison.Equal)
      .FilterCompare("YearOfRelease", query.Year, FilterComparison.Equal)
      .Where(StatusPermissionsFilter(userId));
  }

  /// <summary>
  /// Sorts the elements of a sequence in ascending or descending order according to a request query fields.
  /// </summary>
  /// <param name="query">The request query DTO based sorting operations</param>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static Func<IQueryable<Movie>, IQueryable<Movie>> GetAllSortBy(MoviesGetAllQuery query) {
    return q => {
      if (string.IsNullOrWhiteSpace(query.SortBy))
        return q.OrderByDescending(x => x.CreatedAt);

      var sortOrder = StringHelper.GetSortByFrom(query.SortBy);

      return StringHelper.UnescapeSortOrder(query.SortBy) switch {
        SortByTitle => q.SortBy(sortOrder, x => x.Title),
        SortByYear => q.SortBy(sortOrder, x => x.YearOfRelease),
        _ => q
      };
    };
  }
}