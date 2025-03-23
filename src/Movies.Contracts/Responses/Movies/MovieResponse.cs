// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Ratings;

namespace Movies.Contracts.Responses.Movies;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public record MovieResponse {
  private float _rating;
  [SwaggerSchema("The id of movie", Nullable = false)]
  public long? Id { get; set; }
  [SwaggerSchema("The owner id of movie", Nullable = false)]
  public long? UserId { get; set; }
  [SwaggerSchema("The title of movie", Nullable = false)]
  public string? Title { get; set; }
  [SwaggerSchema("The year of release", Nullable = false)]
  public long? YearOfRelease { get; set; }
  [SwaggerSchema("The slug of movie", Nullable = false)]
  public string? Slug { get; set; }

  [SwaggerSchema("Average rating", Nullable = false)]
  public float? Rating {
    get => _rating;
    set => _rating = float.Round(value ?? 0, 1);
  }

  [SwaggerSchema("User's rating", Nullable = false)]
  public short? UserRating { get; set; }
  [SwaggerSchema("The status of movie", Nullable = false)]
  public string? Status { get; set; }
  [SwaggerSchema("The movie created timestamp", Nullable = false)]
  public DateTime? CreatedAt { get; set; }
  [SwaggerSchema("The movie updated timestamp", Nullable = false)]
  public DateTime? UpdatedAt { get; set; }
  [SwaggerSchema("The genres of movie", Nullable = false)]
  public IEnumerable<GenreResponse> Genres { get; set; } = [];
  [SwaggerSchema("The ratings of movie", Nullable = false)]
  public IEnumerable<RatingResponse> Ratings { get; set; } = [];
}