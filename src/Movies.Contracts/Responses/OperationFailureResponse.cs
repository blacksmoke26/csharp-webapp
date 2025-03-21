// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema(
  "This represents the error response caused by some operation failure", ReadOnly = true)]
public struct OperationFailureResponse {
  [SwaggerSchema("The error code")]
  public required string ErrorCode { get; init; }

  [SwaggerSchema("List of validation errors")]
  public required IEnumerable<OperationFailureError> Errors { get; init; }
}

public struct OperationFailureError {
  [SwaggerSchema("The error message")]
  public required string Message { get; set; }
}