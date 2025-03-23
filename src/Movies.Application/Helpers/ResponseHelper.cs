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
  public static SuccessOnlyResponse SuccessOnly() => new();

  /// <summary>
  /// Returns the successful response with data object
  /// </summary>
  /// <param name="data">The dynamic object</param>
  /// <returns>The response object</returns>
  public static SuccessResponse SuccessWithData(object? data) {
    return new() { Data = data ?? "Nothing to be returned" };
  }

  /// <summary>
  /// Returns the successful response with data object
  /// </summary>
  /// <param name="message">The informational message</param>
  /// <returns>The response object</returns>
  public static SuccessWithMessageResponse SuccessWithMessage(string message) {
    return new() { Message = message };
  }

  /// <summary>
  /// Returns the successful response with the paginated results
  /// </summary>
  /// <param name="instance">The paginated list instance</param>
  /// <returns>The paginated response object</returns>
  public static PaginatedSuccessResponse SuccessWithPaginated<T>(PaginatedList<T> instance)
    => new(instance.ToPaginatedResult());
}