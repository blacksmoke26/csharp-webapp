// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Query;

[SwaggerSchema("Use to filter the list of movies", WriteOnly = true)]
public record MoviesGetAllQuery : RequestQueryFetching {
  [SwaggerSchema("The creator user ID")]
  public long? UserId { get; set; } = null;
  
  [SwaggerSchema("Movie title (case-insensitive search)")]
  public string? Title { get; set; } = null;

  [SwaggerSchema("Year of release")]
  public short? Year { get; set; } = null;
}