// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses.Identity;

[Description("This response class contains the information regarding user and authorization")]
public record UserAuthenticateResponse {
  [Required, JsonPropertyName("auth"), Description("The authorization details")]
  public required AuthTokenResult Auth { get; init; }

  [Required, JsonPropertyName("user"), Description("The user account details")]
  public required UserAuthInfo User { get; init; }
}

[Description("This object represents the authorization information")]
public struct AuthTokenResult {
  [Required, JsonPropertyName("token"), Description("The JWT authorization token")]
  public required string Token { get; init; }

  [Required, JsonPropertyName("issuedAt"),
   Description("The token issued timestamp")]
  public required DateTime IssuedAt { get; init; }

  [Required, JsonPropertyName("expires"),
   Description("The token expiration timestamp")]
  public required DateTime Expires { get; init; }
}

[Description("This object represents the user's authorization information")]
public struct UserAuthInfo {
  [Required, JsonPropertyName("fullname"), Description("The user's first and last name")]
  public required string Fullname { get; init; }

  [Required, JsonPropertyName("firstName"), Description("The user's first name")]
  public required string FirstName { get; init; }

  [Required, JsonPropertyName("lastName"), Description("The user's last name")]
  public required string LastName { get; init; }

  [Required, JsonPropertyName("email"), Description("The email address")]
  public required string Email { get; init; }

  [Required, JsonPropertyName("role"), Description("The role name")]
  public required string Role { get; init; }
}