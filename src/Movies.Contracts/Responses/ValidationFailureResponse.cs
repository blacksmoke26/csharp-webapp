// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

public class ValidationFailureResponse {
  /// <summary>
  /// List of validation errors
  /// </summary>
  public required IEnumerable<ValidationResponse> Errors { get; init; }
}

public class ValidationResponse {
  /// <summary>
  /// Failed validation property name
  /// </summary>
  public required string PropertyName { get; init; }
  
  /// <summary>
  /// The error message associated with the property
  /// </summary>
  public required string Message { get; init; }
}