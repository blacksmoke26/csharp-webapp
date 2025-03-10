// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Dto;

public struct MovieCreateDto {
  /// <summary>
  /// The title of movie
  /// </summary>
  public string Title { get; set; }

  /// <summary>
  /// Official year of release
  /// </summary>
  public short YearOfRelease { get; set; }

  /// <summary>
  /// The list of genres
  /// </summary>
  public IEnumerable<string> Genres { get; set; }
}