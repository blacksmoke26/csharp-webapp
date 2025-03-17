// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

public class PaginatedSuccessResponse(
  PaginatedResult result, bool excludeNullValues = false) : ISuccessResponse {
  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public bool Success => true;

  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public int TotalCount => result.TotalCount;

  /// <summary>
  /// This response class  represents the successful response
  /// containing the `data` property
  /// </summary>
  public object Data => new SuccessResponse(result.Rows, excludeNullValues).Data;

  /// <summary>
  /// This response class  represents the successful response
  /// containing the `data` property
  /// </summary>
  public PageInfo PageInfo => new() {
    CurrentPage = result.CurrentPage,
    TotalPages = result.TotalPages,
    HasPreviousPage = result.HasPreviousPage,
    HasNextPage = result.HasNextPage,
  };
}