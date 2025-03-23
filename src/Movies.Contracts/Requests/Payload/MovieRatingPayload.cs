// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[SwaggerSchema("Use to rate a movie",
  Required = ["Rating"], WriteOnly = true)]
public struct MovieRatingPayload {
  [Required] [SwaggerSchema("Overall rating", Nullable = false)]
  public short Rating { get; init; }

  [SwaggerSchema("Additional feedback")]
  public string? Feedback { get; init; }
}