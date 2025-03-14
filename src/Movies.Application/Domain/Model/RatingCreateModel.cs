// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Model;

public struct RatingCreateModel {
  /// <summary>The user id</summary>
  public required long UserId { get; set; }

  /// <summary>Movie ID</summary>
  public required long MovieId { get; set; }

  /// <summary>Movie ID</summary>
  public required short Score { get; set; }

  /// <summary>Movie ID</summary>
  public string? Feedback { get; set; }
}