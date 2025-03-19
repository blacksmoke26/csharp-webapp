// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Query;

[SwaggerSchema("Use to filter the list of movies", WriteOnly = true)]
public record RatingsGetAllQuery : RequestQueryFetching {
  [SwaggerSchema("The id of the movie")]
  public long? MovieId { get; set; } = null;
  
  [SwaggerSchema("The score")]
  public short? Score { get; set; } = null;
}