// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema("This class formats the validator errors response",
  Required = ["Errors"], ReadOnly = true, Nullable = false)]
public class ValidationFailureResponse {
  [SwaggerSchema("The error code")]
  public string? ErrorCode { get; set; }
  [SwaggerSchema("The error message")]
  public string Message { get; set; } = string.Empty;
  [SwaggerSchema("List of validation errors")]
  public required IEnumerable<ValidationResponse> Errors { get; init; }

  /// <summary>
  /// Transforms the current object into a JSON string representation
  /// </summary>
  /// <returns>The JSON text</returns>
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
          return error;
        }).ToList()
      );
    }

    return obj;
  }
}

[SwaggerSchema("This response class formats the validation error",
  Required = ["Message"], ReadOnly = true)]
public class ValidationResponse {
  [SwaggerSchema("The property name raising error")]
  public string? PropertyName { get; init; }

  [SwaggerSchema("The error message", Nullable = false)]
  public required string Message { get; init; }

  [SwaggerSchema("The error code")]
  public required string? ErrorCode { get; init; }
}