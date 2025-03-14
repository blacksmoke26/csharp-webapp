// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Ratings;

public struct RatingResponse {
  public long? Id { get; set; }
  public long? UserId { get; set; }
  public long? MovieId { get; set; }
  public short? Score { get; set; }
  public string? Feedback { get; set; }
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}