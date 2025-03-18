// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Ratings;

namespace Movies.Contracts.Responses.Movies;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public record MovieResponse {
  private float _rating;
  public long? Id { get; set; }
  public long? UserId { get; set; }
  public string? Title { get; set; }
  public long? YearOfRelease { get; set; }
  public string? Slug { get; set; }

  public float? Rating {
    get => _rating;
    set => _rating = float.Round(value ?? 0, 1);
  }

  public short? UserRating { get; set; }
  public string? Status { get; set; }
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public IEnumerable<GenreResponse> Genres { get; set; } = [];
  public IEnumerable<RatingResponse> Ratings { get; set; } = [];
}