// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Movies;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct GenreResponse {
  public long? Id { get; set; }
  public long? MovieId { get; set; }
  public string? Name { get; set; }
}