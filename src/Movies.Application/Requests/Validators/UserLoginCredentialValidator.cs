// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Requests.Validators;

public class UserLoginCredentialValidator : AbstractValidator<UserLoginCredentialDto> {
  public UserLoginCredentialValidator() {
    RuleFor(x => x.Email)
      .EmailAddress()
      .NotEmpty();

    RuleFor(x => x.Password)
      .MinimumLength(8)
      .MaximumLength(20)
      .NotEmpty();
  }
}