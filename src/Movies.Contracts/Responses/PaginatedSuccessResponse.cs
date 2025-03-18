// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

/// <summary>
/// This response class formats the successful success response using <c>Paginated</c> results.
/// </summary>
/// <param name="result">The PaginatedResult object</param>
public class PaginatedSuccessResponse(PaginatedResult result) : ISuccessResponse {
  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public bool Success => true;

  /// <summary>
  /// The total count of records
  /// </summary>
  public int TotalCount => result.TotalCount;

  /// <summary>
  /// This response class  represents the successful response
  /// containing the list of `entities`
  /// </summary>
  public object Data => new SuccessResponse(result.Rows).Data;

  /// <summary>
  /// The pagination parameters
  /// </summary>
  public PageInfo PageInfo => new() {
    CurrentPage = result.CurrentPage,
    TotalPages = result.TotalPages,
    HasPreviousPage = result.HasPreviousPage,
    HasNextPage = result.HasNextPage,
  };
}