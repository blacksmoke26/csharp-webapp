// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Dto;

public struct MovieRatingDto {
  /// <summary>Movie ID</summary>
  public short Rating { get; init; }

  /// <summary>Movie ID</summary>
  public string? Feedback { get; init; }
}