// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp


namespace Movies.Contracts.Responses;

[SwaggerSchema("This response class formats the successful informational message ",
  Required = ["Success", "Message"], ReadOnly = true)]
public record SuccessWithMessageResponse : ISuccessResponse {
  [SwaggerSchema("The success property means operation was a success")]
  public bool Success => true;

  [SwaggerSchema("The informational message")]
  public required string Message { get; init; }
}