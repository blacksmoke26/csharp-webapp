// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This response class contains the pagination information")]
public struct PageInfo {
  [Required, JsonPropertyName("currentPage"), Description("The current page")]
  public required int CurrentPage { get; init; }

  [Required, JsonPropertyName("totalPages"), Description("Count of total pages")]
  public required int TotalPages { get; init; }

  [Required, JsonPropertyName("hasPreviousPage"), Description("Whatever there is a page before the current page")]
  public required bool HasPreviousPage { get; init; }

  [Required, JsonPropertyName("hasNextPage"), Description("Whatever there is a page after the current page")]
  public required bool HasNextPage { get; init; }
}