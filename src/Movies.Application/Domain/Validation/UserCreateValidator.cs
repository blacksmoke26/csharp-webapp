// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Validation;

public class UserCreateValidator : AbstractValidator<UserCreateModel> {
  private readonly UserRepository _userRepo;

  public UserCreateValidator(UserRepository userRepo) {
    _userRepo = userRepo;

    RuleFor(x => x.FirstName)
      .MinimumLength(3)
      .MaximumLength(20)
      .NotEmpty();

    RuleFor(x => x.LastName)
      .MinimumLength(3)
      .MaximumLength(20)
      .NotEmpty();

    RuleFor(x => x.Email)
      .EmailAddress()
      .NotEmpty();

    RuleFor(x => x.Password)
      .MinimumLength(8)
      .MaximumLength(20)
      .NotEmpty();

    RuleFor(x => x.Email)
      .MustAsync(ValidateEmailAsync)
      .WithMessage("This email address is already registered.");
  }

  /// <summary>
  /// Validates that the provided email address does not exist
  /// </summary>
  /// <param name="userSignup">The input object</param>
  /// <param name="email">Email address</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True when not exist, false otherwise</returns>
  private async Task<bool> ValidateEmailAsync(
    UserCreateModel userSignup, string email, CancellationToken token) {
    return !await _userRepo.ExistsAsync(x => x.Email == email, token);
  }
}