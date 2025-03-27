// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[Description("Use to signup a new user account")]
public struct UserSignupPayload {
  [Required, JsonPropertyName("firstName"), Description("The first name of user")] [property: Range(3, 20)]
  public string FirstName { get; init; }

  [Required, JsonPropertyName("lastName"), Description("The last name of user")] [property: Range(3, 20)]
  public string LastName { get; init; }

  [Required, JsonPropertyName("email"), Description("The email address")] [property: MaxLength(255)]
  public string Email { get; init; }

  [Required, JsonPropertyName("password"), Description("Password with alphanumeric chars along with symbols")]
  [property: Range(8, 20)]
  public string Password { get; init; }
}