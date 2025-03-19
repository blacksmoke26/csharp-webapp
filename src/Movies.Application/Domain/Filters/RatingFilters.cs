// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Core.Extensions;
using Movies.Application.Core.Interfaces;
using Movies.Contracts.Requests.Query;

namespace Movies.Application.Domain.Filters;

public sealed class RatingFilters : IEntityFilters<Rating, RatingsGetAllQuery> {
  public const string SortByScore = "score";
  public const string SortByCreatedAt = "createdAt";
  public const string SortByUpdatedAt = "updatedAt";

  /// <inheritdoc/>
  public static IEnumerable<string> SortByFields { get; } = [
    SortByScore, SortByCreatedAt, SortByUpdatedAt
  ];

  private RatingFilters() {
  }

  /// <inheritdoc/>
  public static Func<IQueryable<Rating>, IQueryable<Rating>>
    GetAllQuery(RatingsGetAllQuery query, long? userId = null) {
    return q => q
      .FilterCompare("UserId", userId, FilterComparison.Equal)
      .FilterCompare("MovieId", query.MovieId, FilterComparison.Equal)
      .FilterCompare("Score", query.Score, FilterComparison.Equal)
      .AddQuery(GetAllSortBy(query));
  }

  /// <inheritdoc/>
  public static Func<IQueryable<Rating>, IQueryable<Rating>> GetAllSortBy(RatingsGetAllQuery query) {
    return q => {
      if (string.IsNullOrWhiteSpace(query.SortBy))
        return q.OrderByDescending(x => x.UpdatedAt);

      var sortOrder = query.GetSortOrder();

      return query.GetSortField(SortByCreatedAt) switch {
        SortByScore => q.SortBy(sortOrder, x => x.Score),
        SortByCreatedAt => q.SortBy(sortOrder, x => x.CreatedAt),
        SortByUpdatedAt => q.SortBy(sortOrder, x => x.UpdatedAt),
        _ => throw new ArgumentOutOfRangeException()
      };
    };
  }
}