// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://stackoverflow.com/questions/6507889/how-to-ignore-a-property-in-class-if-null-using-json-net

namespace Movies.Contracts.Responses.Ratings;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct RatingResponse {
  public long? Id { get; set; }
  public long? UserId { get; set; }
  public long? MovieId { get; set; }
  public MovieRatingResponse? Movie { get; set; }
  public short? Score { get; set; }
  public string? Feedback { get; set; }
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public struct MovieRatingResponse {
  public long? Id { get; set; }
  public string? Title { get; set; }
  public long? YearOfRelease { get; set; }
  public string? Slug { get; set; }
}