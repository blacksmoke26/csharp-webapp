// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema("This response class formats the paginated results ",
  Required = ["Success", "Message"], ReadOnly = true)]
public abstract record PaginatedResult<TEntity> {
  [SwaggerSchema("The current page")] [DefaultValue(1)]
  public int CurrentPage { get; init; }

  [SwaggerSchema("Count of total records")]
  public required int TotalCount { get; init; }

  [SwaggerSchema("Count of total pages")]
  public required int TotalPages { get; init; }

  [SwaggerSchema("Whatever there is a page before the current page")] [DefaultValue(false)]
  public required bool HasPreviousPage { get; init; }

  [SwaggerSchema("Whatever there is a page after the current page")] [DefaultValue(false)]
  public required bool HasNextPage { get; init; }

  [SwaggerSchema("List of entities")]
  public required IEnumerable<TEntity> Rows { get; init; }
}

[SwaggerSchema("This response class formats the paginated results ", ReadOnly = true)]
public record PaginatedResult : PaginatedResult<object> {
}