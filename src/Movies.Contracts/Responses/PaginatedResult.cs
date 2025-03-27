// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This response class formats the paginated results")]
public abstract record PaginatedResult<TEntity> {
  [JsonPropertyName("currentPage"), Description("The current page")] [DefaultValue(1)]
  public int CurrentPage { get; init; }

  [Required, JsonPropertyName("totalCount"), Description("Count of total records")]
  public required int TotalCount { get; init; }

  [Required, JsonPropertyName("totalPages"), Description("Count of total pages")]
  public required int TotalPages { get; init; }

  [Required, JsonPropertyName("hasPreviousPage"), Description("Whatever there is a page before the current page")] [DefaultValue(false)]
  public required bool HasPreviousPage { get; init; }

  [Required, JsonPropertyName("hasNextPage"), Description("Whatever there is a page after the current page")] [DefaultValue(false)]
  public required bool HasNextPage { get; init; }

  [Required, JsonPropertyName("rows"), Description("List of entities")]
  public required IEnumerable<TEntity> Rows { get; init; }
}

[Description("This response class formats the paginated results")]
public record PaginatedResult : PaginatedResult<object> {
}