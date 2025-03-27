// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This represents the error response caused by some operation failure")]
public struct OperationFailureResponse {
  [Required, JsonPropertyName("errorCode"), Description("The error code")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string ErrorCode { get; init; }

  [Required, JsonPropertyName("errors"), Description("List of validation errors")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required IEnumerable<OperationFailureError> Errors { get; init; }
}

public struct OperationFailureError {
  [Required, JsonPropertyName("message"), Description("The error message")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Message { get; set; }
}