// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

[SwaggerSchema("This response class formats the successful success response using dynamic data.",
  Required = ["Success", "Data"], ReadOnly = true)]
public record SuccessResponse<T> : ISuccessResponse {
  [SwaggerSchema("The success property means operation was a success")]
  public bool Success => true;

  [SwaggerSchema("The information / processed data")]
  public required T Data { get; init; }
}

[SwaggerSchema("This response class formats the successful success response using dynamic data.",
  Required = ["Success", "Data"], ReadOnly = true)]
public record SuccessResponse : SuccessResponse<object> {
}