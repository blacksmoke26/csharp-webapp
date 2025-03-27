// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Ratings;

namespace Movies.Contracts.Responses.Movies;

public record MovieResponse {
  private float _rating;

  [JsonPropertyName("id"), Description("The id of movie")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? Id { get; set; }

  [JsonPropertyName("userId"), Description("The owner id of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? UserId { get; set; }

  [JsonPropertyName("title"), Description("The title of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Title { get; set; }

  [JsonPropertyName("yearOfRelease"), Description("The year of release")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public short? YearOfRelease { get; set; }

  [JsonPropertyName("slug"), Description("The slug of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Slug { get; set; }

  [JsonPropertyName("rating"), Description("Average rating")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public float? Rating {
    get => _rating;
    set => _rating = float.Round(value ?? 0, 1);
  }

  [JsonPropertyName("userRating"), Description("User's rating")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public short? UserRating { get; set; }

  [JsonPropertyName("status"), Description("The status of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Status { get; set; }

  [JsonPropertyName("createdAt"), Description("The movie created timestamp")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public DateTime? CreatedAt { get; set; }

  [JsonPropertyName("updatedAt"), Description("The movie updated timestamp")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public DateTime? UpdatedAt { get; set; }

  [JsonPropertyName("genres"), Description("The genres of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public IEnumerable<GenreResponse> Genres { get; set; } = [];

  [JsonPropertyName("ratings"), Description("The ratings of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public IEnumerable<RatingResponse> Ratings { get; set; } = [];
}