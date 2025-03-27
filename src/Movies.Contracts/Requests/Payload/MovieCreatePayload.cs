// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[Description("Use to create a movie")]
public struct MovieCreatePayload {
  [Required, JsonPropertyName("title"), Description("The title of movie")]
  [property: Range(3, 60)]
  public string Title { get; set; }

  [Required, JsonPropertyName("yearOfRelease"), Description("Official year of release")]
  [property: Range(1950, 2025)]
  public short YearOfRelease { get; set; }

  [Required, JsonPropertyName("genres"), Description("The list of genres")]
  [property: Range(1, 5)]
  public IEnumerable<string> Genres { get; set; }
}