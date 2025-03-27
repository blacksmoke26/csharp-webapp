// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This response class formats the successful success response containing paginated results.")]
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

  [JsonPropertyName("success"), Description("The operation was successful")]
  public bool Success => true;

  [JsonPropertyName("totalCount"), Description("The total count of records")]
  public int TotalCount => Result.TotalCount;

  [JsonPropertyName("data"), Description("The list of entities")]
  public IEnumerable<TEntity> Data => Result.Rows.Cast<TEntity>();

  [JsonPropertyName("pageInfo"), Description("The pagination information")]
  public PageInfo PageInfo => new() {
    CurrentPage = Result.CurrentPage,
    TotalPages = Result.TotalPages,
    HasPreviousPage = Result.HasPreviousPage,
    HasNextPage = Result.HasNextPage,
  };
}

[Description("This response class formats the successful success response containing paginated results.")]
public record PaginatedSuccessResponse : PaginatedSuccessResponse<object> {
  public PaginatedSuccessResponse(PaginatedResult result) {
    Result = result;
  }
}