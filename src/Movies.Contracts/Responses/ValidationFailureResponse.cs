// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

public class ValidationFailureResponse {
  public string? ErrorCode { get; set; }
  public string Message { get; set; } = string.Empty;

  /// <summary>
  /// List of validation errors
  /// </summary>
  public required IEnumerable<ValidationResponse> Errors { get; init; }

  public object ToJson() {
    Dictionary<string, object> obj = [];
    obj.Add("data", null!);

    if (!string.IsNullOrWhiteSpace(Message) && !Message.StartsWith("Validation failed:"))
      obj.Add("message", Message);

    if (ErrorCode != null) obj.Add("errorCode", ErrorCode);

    if (Errors.Any()) {
      obj.Add("errors", Errors.Select(x => {
          Dictionary<string, object> error = [];

          if (x.ErrorCode != null)
            error.Add("errorCode",
              x.ErrorCode.EndsWith("Validator")
                ? "VALIDATION_ERROR" : x.ErrorCode
            );

          if (x.PropertyName != null)
            error.Add("propertyName", x.PropertyName);

          error.Add("message", x.Message);

          if (x.AttemptedValue != null)
            error.Add("attemptedValue", x.AttemptedValue);
          return error;
        }).ToList()
      );
    }

    return obj;
  }
}

public class ValidationResponse {
  /// <summary>
  /// Failed validation property name
  /// </summary>
  public string? PropertyName { get; init; }

  /// <summary>
  /// The error message
  /// </summary>
  public required string Message { get; init; }

  /// <summary>
  /// The property value that caused the failure.
  /// </summary>
  public object? AttemptedValue { get; init; }

  /// <summary>
  /// Gets or sets the error code.
  /// </summary>
  public string? ErrorCode { get; init; }
}