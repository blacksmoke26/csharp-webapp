// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Query;

public record MoviesGetAllQuery : RequestQueryFetching {
  /// <summary>The creator user ID</summary>
  public long? UserId { get; set; } = null;
  
  /// <summary>Movie title (case-insensitive lossy search)</summary>
  public string? Title { get; set; } = null;

  /// <summary>Year of release</summary>
  public short? Year { get; set; } = null;
}