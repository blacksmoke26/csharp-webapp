// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[Description("Use to update a movie")]
public struct MovieUpdatePayload {
  [Required, JsonPropertyName("title"), Description("The title of movie")]
  [property: Range(3, 60)]
  public string Title { get; set; }

  [Required, JsonPropertyName("yearOfRelease"), Description("Official year of release")]
  [property: Range(1950, 2025)]
  public short YearOfRelease { get; set; }

  [property: Range(1, 5)]
  [Required, JsonPropertyName("genres"), Description("The list of genres")]
  public IEnumerable<string> Genres { get; set; }
}