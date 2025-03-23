// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Core.Extensions;
using Movies.Application.Core.Interfaces;
using Movies.Contracts.Requests.Query;

namespace Movies.Application.Domain.Filters;

public sealed class MovieFilters : IEntityFilters<Movie, MoviesGetAllQuery> {
  public const string SortByTitle = "title";
  public const string SortByYear = "year";
  public const string SortByCreatedAt = "createdAt";

  public static short YearMin => 1900;
  public static short YearMax => (short)DateTime.UtcNow.Year;

  /// <inheritdoc/>
  public static IEnumerable<string> SortByFields => [
    SortByTitle, SortByYear, SortByCreatedAt
  ];

  private MovieFilters() {
  }

  /// <summary>List of status only applicable for owner/creator users</summary>
  public static MovieStatus[] CreatorStatuses => [
    MovieStatus.Draft, MovieStatus.Pending, MovieStatus.Published
  ];

  /// <summary>List of status only applicable for end users / anonymous users</summary>
  public static MovieStatus[] ReaderStatuses => [
    MovieStatus.Published
  ];

  /// <summary>Filters the records against reader or creator statuses</summary>
  /// <param name="userId">The user ID</param>
  /// <returns>A function to test each element for a condition.</returns>
  public static Expression<Func<Movie, bool>> StatusPermissionsFilter(long? userId)
    => x => x.UserId == userId
      ? CreatorStatuses.Contains(x.Status)
      : ReaderStatuses.Contains(x.Status);

  /// <inheritdoc/>
  public static Func<IQueryable<Movie>, IQueryable<Movie>> GetAllQuery(
    MoviesGetAllQuery query, long? userId = null) {
    return q => q
      .AsNoTracking()
      .FilterILike("Title", query.Title)
      .FilterCompare("UserId", query.UserId, FilterComparison.Equal)
      .FilterCompare("YearOfRelease", query.Year, FilterComparison.Equal)
      .Where(StatusPermissionsFilter(userId))
      .AddQuery(GetAllSortBy(query));
  }

  /// <inheritdoc/>
  public static Func<IQueryable<Movie>, IQueryable<Movie>> GetAllSortBy(MoviesGetAllQuery query) {
    return q => {
      if (string.IsNullOrWhiteSpace(query.SortBy))
        return q.OrderByDescending(x => x.CreatedAt);

      var sortOrder = query.GetSortOrder();

      return query.GetSortField(SortByCreatedAt) switch {
        SortByTitle => q.SortBy(sortOrder, x => x.Title),
        SortByYear => q.SortBy(sortOrder, x => x.YearOfRelease),
        SortByCreatedAt => q.SortBy(sortOrder, x => x.CreatedAt),
        _ => throw new ArgumentOutOfRangeException()
      };
    };
  }
}