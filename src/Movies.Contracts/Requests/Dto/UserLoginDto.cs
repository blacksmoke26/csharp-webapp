// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Dto;

public struct UserLoginCredentialDto {
  public string Email { get; init; }
  public string Password { get; init; }
}

public struct UserLoginClaimDto {
  public string Jti { get; init; }
  public string Role { get; init; }
}