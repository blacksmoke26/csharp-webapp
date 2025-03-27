// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Movies;

[Description("The movie genre")]
public struct GenreResponse {
  [JsonPropertyName("id"), Description("The id of genre")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? Id { get; set; }

  [JsonPropertyName("movieId"), Description("The movie id of genre")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? MovieId { get; set; }

  [JsonPropertyName("name"), Description("The name of genre")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Name { get; set; }
}