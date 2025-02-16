// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using CaseConverter;

namespace Movies.Application.Models;

public class Movie {
  public long? Id { get; set; }

  // section Test
  public required string Title { get; set; }

  public required int YearOfRelease { get; set; }

  public string Slug => GenerateSlug();

  public required List<string> Genres { get; init; } = [];

  private string GenerateSlug() {
    return $"{Title} {YearOfRelease}".ToKebabCase();
  }
}