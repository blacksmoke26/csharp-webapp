// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[Description("This response class formats the successful success response using dynamic data.")]
public record SuccessResponse<T> : ISuccessResponse {
  [JsonPropertyName("success"), Description("The operation was successful")]
  public bool Success => true;

  [Required, JsonPropertyName("data"), Description("The processed data")]
  public required T Data { get; init; }
}

[Description("This response class formats the successful success response using dynamic data.")]
public record SuccessResponse : SuccessResponse<object> {
}