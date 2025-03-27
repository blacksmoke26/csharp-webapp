// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Query;

[Description("Use to filter the list of movies")]
public record RatingsGetAllQuery : RequestQueryFetching {
  [JsonPropertyName("movieId"), Description("The id of the movie")] [property: MinLength(1)]
  public long? MovieId { get; set; } = null;

  [JsonPropertyName("score"), Description("The score")] [property: Range(0, 5)]
  public short? Score { get; set; } = null;
}