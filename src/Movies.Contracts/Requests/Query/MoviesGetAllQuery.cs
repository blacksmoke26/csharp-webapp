// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Query;

[Description("Use to filter the list of movies")]
public record MoviesGetAllQuery : RequestQueryFetching {
  [JsonPropertyName("userId"), Description("The creator user ID")] [property: MinLength(1)]
  public long? UserId { get; set; } = null;

  [JsonPropertyName("title"), Description("Movie title (case-insensitive search)")] [property: Range(1, 60)]
  public string? Title { get; set; } = null;

  [JsonPropertyName("year"), Description("Year of release")] [property: Range(1950, 2025)]
  public short? Year { get; set; } = null;
}