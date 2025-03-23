// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Movies;

[SwaggerSchema("The movie genre", ReadOnly = true)]
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct GenreResponse {
  [SwaggerSchema("The id of genre", ReadOnly = true)]
  public long? Id { get; set; }
  [SwaggerSchema("The movie id of genre", ReadOnly = true)]
  public long? MovieId { get; set; }
  [SwaggerSchema("The name of genre", ReadOnly = true)]
  public string? Name { get; set; }
}