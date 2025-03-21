// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload.Identity;

[SwaggerSchema("Use to reset the account password", WriteOnly = true)]
public record PasswordResetPayload {
  [Required] [SwaggerSchema("The reset code", Format = "int", Nullable = false)]
  [DefaultValue("0467AX")]
  public required string ResetCode { get; set; } = null!;
  
  [Required]
  [DefaultValue("john.doe@domain.com")]
  [SwaggerSchema("The email address", Format = "email", Nullable = false)]
  public required string Email { get; set; } = null!;

  [Required] [SwaggerSchema("The new password", Nullable = false)]
  [DefaultValue("password")]
  public required string NewPassword { get; set; } = null!;
}