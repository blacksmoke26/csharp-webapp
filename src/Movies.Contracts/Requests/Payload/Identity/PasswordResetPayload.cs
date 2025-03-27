// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload.Identity;

[Description("Use to reset the account password")]
public record PasswordResetPayload {
  [Required, JsonPropertyName("resetCode"), Description("The reset code")]
  [property: Range(6, 10)]
  public required string ResetCode { get; set; } = null!;
  
  [Required, JsonPropertyName("email"), Description("The email address")]
  [property: MaxLength(255)]
  public required string Email { get; set; } = null!;

  [Required, JsonPropertyName("newPassword"), Description("The new password")]
  [property: Range(8, 20)]
  public required string NewPassword { get; set; } = null!;
}