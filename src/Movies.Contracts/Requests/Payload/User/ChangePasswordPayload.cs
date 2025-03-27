// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload.User;

[Description("Use to change the account password")]
public struct ChangePasswordPayload {
  [Required, JsonPropertyName("CurrentPassword"), Description("The current password")] [property: Range(8, 20)]
  public string CurrentPassword { get; set; }

  [Required, JsonPropertyName("NewPassword"), Description("The new password to update")] [property: Range(8, 20)]
  public string NewPassword { get; set; }
}