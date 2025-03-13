// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using FluentValidation.Results;

namespace Movies.Application.Helpers;

public static class ValidationHelper {
  public const string StatusCodeKey = "$Context(StatusCode)";
  public const string ErrorCodeKey = "$Context(ErrorCode)";

  /// <summary>
  /// Creates a validation error
  /// </summary>
  /// <param name="errors">List of validation errors</param>
  /// <returns>The ValidationException instance</returns>
  public static ValidationException Create(IEnumerable<ValidationFailure> errors) {
    return new ValidationException(string.Empty, errors);
  }

  /// <summary>
  /// Creates a validation error with HTTP Status
  /// </summary>
  /// <param name="errors">List of validation errors</param>
  /// <param name="statusCode">HTTP Status code (e.g., 404) </param>
  /// <returns>The ValidationException instance</returns>
  public static ValidationException Create(IEnumerable<ValidationFailure> errors, int statusCode) {
    var exception = Create(errors);
    exception.Data.Add(StatusCodeKey, statusCode);
    return exception;
  }

  /// <summary>
  /// Creates a validation error with Error code
  /// </summary>
  /// <param name="errors">List of validation errors</param>
  /// <param name="errorCode">The error code (e.g., "BAD_REQUEST")</param>
  /// <returns>The ValidationException instance</returns>
  public static ValidationException Create(IEnumerable<ValidationFailure> errors, string errorCode) {
    var exception = Create(errors);
    exception.Data.Add(ErrorCodeKey, errorCode);
    return exception;
  }

  /// <summary>
  /// Creates a validation error with HTTP Status and Error code
  /// </summary>
  /// <param name="errors">List of validation errors</param>
  /// <param name="statusCode">HTTP Status code (e.g., 404) </param>
  /// <param name="errorCode">The error code (e.g., "BAD_REQUEST")</param>
  /// <returns>The ValidationException instance</returns>
  public static ValidationException Create(IEnumerable<ValidationFailure> errors, int statusCode, string errorCode) {
    var exception = Create(errors, errorCode);
    exception.Data.Add(StatusCodeKey, statusCode);
    return exception;
  }
}