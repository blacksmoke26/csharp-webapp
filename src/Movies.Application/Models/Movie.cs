// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Models;

public class Movie {
  public required Guid Id { get; init; }

  // section Test
  public required string Title { get; set; }
  
  public required int YearOfRelease { get; set; }
  
  public required List<string> Genres { get; init; } = [];
}