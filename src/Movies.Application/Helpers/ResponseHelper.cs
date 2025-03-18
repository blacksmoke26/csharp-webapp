// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Contracts.Responses;

namespace Movies.Application.Helpers;

/// <summary>
/// Provides the utility methods for formatting the successful responses
/// </summary>
public static class ResponseHelper {
  /// <summary>
  /// Returns the successful response
  /// </summary>
  /// <returns>The response object</returns>
  public static ISuccessResponse SuccessOnly() => new SuccessOnlyResponse();

  /// <summary>
  /// Returns the successful response with data object
  /// </summary>
  /// <param name="data">The dynamic object</param>
  /// <returns>The response object</returns>
  public static ISuccessResponse SuccessWithData(object? data) {
    return data is null ? SuccessOnly() : new SuccessResponse(data);
  }

  /// <summary>
  /// Returns the successful response with the paginated results
  /// </summary>
  /// <param name="result">The paginated results object</param>
  /// <returns>The paginated response object</returns>
  public static PaginatedSuccessResponse SuccessWithPaginated
    (PaginatedResult result) => new(result);
}