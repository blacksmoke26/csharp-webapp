// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[SwaggerSchema("Use to update a movie",
  Required = ["Title", "YearOfRelease", "Genres"], WriteOnly = true)]
public struct MovieUpdatePayload {
  [Required] [DefaultValue("Face Off")] [SwaggerSchema("The title of movie", Nullable = false)]
  public string Title { get; set; }

  [Required] [DefaultValue("2025")] [SwaggerSchema("Official year of release", Nullable = false)]
  public short YearOfRelease { get; set; }

  [Required] [SwaggerSchema("The list of genres", Nullable = false)]
  public IEnumerable<string> Genres { get; set; }
}