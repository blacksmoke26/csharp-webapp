// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://stackoverflow.com/questions/6507889/how-to-ignore-a-property-in-class-if-null-using-json-net

namespace Movies.Contracts.Responses.Ratings;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct RatingResponse {
  [SwaggerSchema("The id of rating", Nullable = false)]
  public long? Id { get; set; }

  [SwaggerSchema("The id of user rated", Nullable = false)]
  public long? UserId { get; set; }

  [SwaggerSchema("The id of movie", Nullable = false)]
  public long? MovieId { get; set; }

  [SwaggerSchema("Overall score", Nullable = false)]
  public short? Score { get; set; }

  [SwaggerSchema("Additional feedback")]
  public string? Feedback { get; set; }

  [SwaggerSchema("The rating created timestamp", Nullable = false)]
  public DateTime? CreatedAt { get; set; }

  [SwaggerSchema("The rating updated timestamp", Nullable = false)]
  public DateTime? UpdatedAt { get; set; }

  [SwaggerSchema("The movie associated with this rating", Nullable = false)]
  public MovieRatingResponse? Movie { get; set; }
}

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct MovieRatingResponse {
  [SwaggerSchema("The id of movie", Nullable = false)]
  public long? Id { get; set; }

  [SwaggerSchema("The title of movie", Nullable = false)]
  public string? Title { get; set; }

  [SwaggerSchema("The year of release", Nullable = false)]
  public short? YearOfRelease { get; set; }

  [SwaggerSchema("The slug of movie", Nullable = false)]
  public string? Slug { get; set; }
}