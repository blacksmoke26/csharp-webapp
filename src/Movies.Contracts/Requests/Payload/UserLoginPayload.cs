// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[Description("Use to login the existing user with credentials")]
public struct UserLoginCredentialPayload {
  [Required, JsonPropertyName("email"), Description("The email address")] [property: MaxLength(255)]
  public required string Email { get; init; }

  [Required, JsonPropertyName("password"), Description("The account password")] [property: Range(8, 20)]
  public required string Password { get; init; }
}

public struct UserLoginClaimPayload {
  public string Jti { get; init; }
  public string Role { get; init; }
}