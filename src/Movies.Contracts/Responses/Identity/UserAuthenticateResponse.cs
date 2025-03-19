// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Identity;

[SwaggerSchema("This response class contains the information regarding user and authorization",
  Required = ["Auth", "User"], ReadOnly = true)]
public record UserAuthenticateResponse {
  [Required] [SwaggerSchema("The authorization details", Nullable = false)]
  public required AuthTokenResult Auth { get; init; }

  [Required] [SwaggerSchema("The user account details", Nullable = false)]
  public required UserAuthInfo User { get; init; }
}

[SwaggerSchema("This object represents the authorization information",
  ReadOnly = true)]
public struct AuthTokenResult {
  [Required] [SwaggerSchema("The JWT authorization token", Nullable = false)]
  public required string Token { get; init; }

  [Required] [SwaggerSchema("The token issued timestamp", Format = "timestamp", Nullable = false)]
  public required DateTime IssuedAt { get; init; }

  [Required] [SwaggerSchema("The token expiration timestamp", Format = "timestamp", Nullable = false)]
  public required DateTime Expires { get; init; }
}

[SwaggerSchema("This object represents the user's authorization information",
  ReadOnly = true)]
public struct UserAuthInfo {
  [Required] [SwaggerSchema("The user's first and last name", Nullable = false)]
  public required string Fullname { get; init; }

  [Required] [SwaggerSchema("The user's first name", Nullable = false)]
  public required string FirstName { get; init; }

  [Required] [SwaggerSchema("The user's last name", Nullable = false)]
  public required string LastName { get; init; }

  [Required] [SwaggerSchema("The email address", Format = "email", Nullable = false)]
  public required string Email { get; init; }

  [Required] [SwaggerSchema("The role name", Nullable = false)]
  public required string Role { get; init; }
}