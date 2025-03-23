// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Payload;

[SwaggerSchema("Use to signup a new user account",
  Required = ["FirstName", "LastName", "Email", "Password"], WriteOnly = true)]
public struct UserSignupPayload {
  [Required] [DefaultValue("John")] [SwaggerSchema("The user's first name", Nullable = false)]
  public string FirstName { get; init; }

  [Required] [DefaultValue("Doe")] [SwaggerSchema("The user's last name", Nullable = false)]
  public string LastName { get; init; }

  [Required]
  [DefaultValue("john.doe@domain.com")]
  [SwaggerSchema("The email address", Format = "email", Nullable = false)]
  public string Email { get; init; }

  [Required] [DefaultValue("<password>")] [SwaggerSchema("Password with symbols", Nullable = false)]
  public string Password { get; init; }
}