// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[Description("Use to rate a movie")]
public struct MovieRatingPayload {
  [Required, JsonPropertyName("rating"), Description("Overall rating")] [property: Range(0, 5)]
  public short Rating { get; init; }

  [JsonPropertyName("feedback"), Description("Additional feedback")]
  [property: MaxLength(1000)]
  public string? Feedback { get; init; }
}