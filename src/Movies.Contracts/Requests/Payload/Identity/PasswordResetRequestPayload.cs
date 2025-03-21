// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload.Identity;

[SwaggerSchema("Use to send a request for a password reset", WriteOnly = true)]
public struct PasswordResetRequestPayload {
  [Required]
  [DefaultValue("john.doe@domain.com")]
  [SwaggerSchema("The email address", Format = "email", Nullable = false)]
  public string Email { get; init; }
}