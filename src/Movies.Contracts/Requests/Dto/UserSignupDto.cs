// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Requests.Dto;

public struct UserSignupDto {
  public string FirstName { get; init; }
  public string LastName { get; init; }
  public string Email { get; init; }
  public string Password { get; init; }
}