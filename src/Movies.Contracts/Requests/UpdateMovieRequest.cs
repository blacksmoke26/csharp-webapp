// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests;

/// <summary>
/// Update request fields mapper interface
/// <strong>Note:</strong> None of these fields are required due to the validations
/// </summary>
public class UpdateMovieRequest {
  /// <summary>
  /// Movie title
  /// </summary>
  public string Title { get; set; } = string.Empty;
  
  /// <summary>
  /// Official year of release
  /// </summary>
  public int YearOfRelease { get; set; }
  
  /// <summary>
  /// Genres list
  /// </summary>
  public IEnumerable<string> Genres { get; init; } = [];
}