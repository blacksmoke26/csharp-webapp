// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Contracts.Responses;

namespace Movies.Application.Helpers;

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
  /// <param name="excludeNullValues">True will remove properties with null values, false will include 'em</param>
  /// <returns>The response object</returns>
  public static ISuccessResponse SuccessWithData(
    object? data, bool excludeNullValues = false) {
    return data is null
      ? SuccessOnly()
      : new SuccessResponse(data, excludeNullValues);
  }

  /// <summary>
  /// Returns the successful response with the paginated results
  /// </summary>
  /// <param name="result">The paginated results object</param>
  /// <param name="excludeNullValues">True will remove properties with null values, false will include 'em</param>
  /// <returns>The paginated response object</returns>
  public static PaginatedSuccessResponse SuccessWithPaginated(
    PaginatedResult result, bool excludeNullValues = false) => new (result, excludeNullValues);
}