// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Domain.Filters;

public static class MovieFilters {
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
}