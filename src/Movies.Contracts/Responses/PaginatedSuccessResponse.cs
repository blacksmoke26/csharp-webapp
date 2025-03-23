// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema("This response class formats the successful success response containing paginated results.",
  ReadOnly = true)]
public record PaginatedSuccessResponse<TEntity> : ISuccessResponse {
  protected PaginatedResult Result = null!;

  /// <param name="result">The PaginatedResult object</param>
  public PaginatedSuccessResponse(PaginatedResult? result = null) {
    Result = result ?? new() {
      Rows = [],
      TotalCount = 0,
      TotalPages = 0,
      HasNextPage = false,
      HasPreviousPage = false
    };
  }

  [SwaggerSchema("The success property means operation was a success")]
  public bool Success => true;

  [SwaggerSchema("The total count of records")]
  public int TotalCount => Result.TotalCount;

  [SwaggerSchema("This response class  represents the successful response containing the list of `entities`")]
  public IEnumerable<TEntity> Data => Result.Rows.Cast<TEntity>();

  [SwaggerSchema("The pagination information")]
  public PageInfo PageInfo => new() {
    CurrentPage = Result.CurrentPage,
    TotalPages = Result.TotalPages,
    HasPreviousPage = Result.HasPreviousPage,
    HasNextPage = Result.HasNextPage,
  };
}

[SwaggerSchema("This response class formats the successful success response containing paginated results.",
  ReadOnly = true)]
public record PaginatedSuccessResponse : PaginatedSuccessResponse<object> {
  public PaginatedSuccessResponse(PaginatedResult result) {
    Result = result;
  }
}