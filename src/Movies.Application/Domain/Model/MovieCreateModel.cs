// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Model;

public struct MovieCreateModel {
  /// <summary>
  /// The creator user ID 
  /// </summary>
  public required long UserId { get; set; }

  /// <summary>
  /// Movie title
  /// </summary>
  public required string Title { get; set; }

  /// <summary>
  /// Year of release (e.g., 2012)
  /// </summary>
  public required short YearOfRelease { get; set; }

  public short? Status { get; set; }

  /// <summary>
  /// List of genres (e.g., ["Comedy", "Horror"])
  /// </summary>
  public required IEnumerable<string> Genres { get; set; }
}