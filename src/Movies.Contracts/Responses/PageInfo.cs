// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema("This response class contains the pagination information",
  Required = ["CurrentPage", "TotalPages", "HasPreviousPage", "HasNextPage"], ReadOnly = true)]
public struct PageInfo {
  [SwaggerSchema("The current page")]
  public required int CurrentPage { get; init; }
  
  [SwaggerSchema("Count of total pages")]
  public required int TotalPages { get; init; }
  
  [SwaggerSchema("Whatever there is a page before the current page")]
  public required bool HasPreviousPage { get; init; }
  
  [SwaggerSchema("Whatever there is a page after the current page")]
  public required bool HasNextPage { get; init; }
}