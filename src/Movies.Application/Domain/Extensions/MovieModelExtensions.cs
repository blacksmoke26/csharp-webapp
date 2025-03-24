// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Extensions;

public static class MovieModelExtensions {
  /// <summary>
  /// Generates a slug against current values
  /// </summary>
  public static void GenerateSlug(this Movie movie) {
    movie.Slug = GenerateSlug(movie, movie.Title, movie.YearOfRelease);
  }

  /// <summary>
  /// Generates a slug based on a movie name and the year of release
  /// </summary>
  public static string GenerateSlug(this Movie movie, string title, short yearOfRelease) {
    return movie.Slug = StringHelper.GenerateSlug(title, " ", yearOfRelease);
  }
}