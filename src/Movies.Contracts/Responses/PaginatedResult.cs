// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

public struct PaginatedResult {
  public int CurrentPage { get; init; }
  public int TotalPages { get; init; }
  public int TotalCount { get; init; }
  public bool HasPreviousPage { get; init; }
  public bool HasNextPage { get; init; }
  public IEnumerable<object> Rows { get; init; }
}