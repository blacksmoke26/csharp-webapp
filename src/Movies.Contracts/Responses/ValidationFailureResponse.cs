// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This class formats the validator errors response")]
public class ValidationFailureResponse {
  [JsonPropertyName("errorCode"), Description("The error code")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? ErrorCode { get; set; }

  [JsonPropertyName("message"), Description("The error message")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string Message { get; set; } = string.Empty;

  [JsonPropertyName("errors"), Description("List of validation errors")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required IEnumerable<ValidationResponse> Errors { get; init; }

  /// <summary>
  /// Transforms the current object into a JSON string representation
  /// </summary>
  /// <returns>The JSON text</returns>
  public object ToJson() {
    Dictionary<string, object> obj = [];
    if (!string.IsNullOrWhiteSpace(Message) && !Message.StartsWith("Validation failed:"))
      obj.Add("message", Message);

    if (ErrorCode != null) obj.Add("errorCode", ErrorCode);

    if (Errors.Any()) {
      obj.Add("errors", Errors.Select(x => {
          Dictionary<string, object> error = [];

          if (x.ErrorCode != null)
            error.Add("errorCode",
              x.ErrorCode.EndsWith("Validator")
                ? "VALIDATION_ERROR"
                : x.ErrorCode
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

[Description("This response class formats the validation error")]
public class ValidationResponse {
  [JsonPropertyName("propertyName"), Description("The raising name which raises the error")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public string? PropertyName { get; init; }

  [Required, JsonPropertyName("message"), Description("The error message")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Message { get; init; }

  [Required, JsonPropertyName("errorCode"), Description("The error code")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string? ErrorCode { get; init; }
}