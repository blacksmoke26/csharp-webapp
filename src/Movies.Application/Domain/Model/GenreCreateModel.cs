// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Model;

public struct GenreCreateModel {
  /// <summary>
  /// The owner movie ID 
  /// </summary>
  public required long MovieId { get; set; }
  
  /// <summary>
  /// The name of genre 
  /// </summary>
  public required string Name { get; set; }
}