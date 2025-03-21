// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload.User;

[SwaggerSchema("Use to change the account password", WriteOnly = true)]
public struct ChangePasswordPayload {
  [Required] [SwaggerSchema("The current password", Nullable = false)]
  public string CurrentPassword { get; set; }

  [Required] [SwaggerSchema("The new password to update", Nullable = false)]
  public string NewPassword { get; set; }
}