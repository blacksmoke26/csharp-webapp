// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[SwaggerSchema("Use to login the existing user with credentials",
  Required = ["Email", "Password"], WriteOnly = true)]
public struct UserLoginCredentialPayload {
  [SwaggerSchema("The email address", Format = "email", Nullable = false)] [Required] [DefaultValue("john.doe@example")]
  public required string Email { get; init; }

  [SwaggerSchema("The account password", Format = "password", Nullable = false)] [Required] [DefaultValue("<password>")]
  public required string Password { get; init; }
}

public struct UserLoginClaimPayload {
  public string Jti { get; init; }
  public string Role { get; init; }
}