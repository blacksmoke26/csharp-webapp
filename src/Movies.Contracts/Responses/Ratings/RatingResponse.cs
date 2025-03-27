// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://stackoverflow.com/questions/6507889/how-to-ignore-a-property-in-class-if-null-using-json-net

namespace Movies.Contracts.Responses.Ratings;

public struct RatingResponse {
  [JsonPropertyName("id"), Description("The id of rating")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? Id { get; set; }

  [JsonPropertyName("userId"), Description("The id of user rated")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? UserId { get; set; }

  [JsonPropertyName("movieId"), Description("The id of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? MovieId { get; set; }

  [JsonPropertyName("score"), Description("Overall score")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  [property: Range(0, 5)]
  public short? Score { get; set; }

  [JsonPropertyName("feedback"), Description("Additional feedback")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Feedback { get; set; }

  [JsonPropertyName("createdAt"), Description("The rating created timestamp")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public DateTime? CreatedAt { get; set; }

  [JsonPropertyName("updatedAt"), Description("The rating updated timestamp")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public DateTime? UpdatedAt { get; set; }

  [JsonPropertyName("movie"), Description("The movie associated with this rating")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public MovieRatingResponse? Movie { get; set; }
}

public struct MovieRatingResponse {
  [JsonPropertyName("id"), Description("The id of movie")] [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public long? Id { get; set; }

  [JsonPropertyName("title"), Description("The title of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Title { get; set; }

  [JsonPropertyName("yearOfRelease"), Description("The year of release")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public short? YearOfRelease { get; set; }

  [JsonPropertyName("slug"), Description("The slug of movie")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? Slug { get; set; }
}