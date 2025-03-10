// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Application.Core.Bases;
using Movies.Application.Helpers;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests.Dto;

namespace Movies.Application.Services;

public struct LoginOptions {
  public string? IpAddress { get; set; }
}

public class IdentityService(
  UserService userService,
  IValidator<UserLoginCredentialDto> loginValidator
): ServiceBase {
  /// <summary>
  /// Logins the user by email and password
  /// </summary>
  /// <param name="input">The user DTO object</param>
  /// <param name="options">Additional login options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User?> LoginAsync(
    UserLoginCredentialDto input, LoginOptions? options = null,
    CancellationToken token = default
  ) {
    await loginValidator.ValidateAndThrowAsync(input, token);

    var user = await userService.GetByEmailAsync(input.Email, token);

    if (user == null || !user.ValidatePassword(input.Password))
      throw ValidationHelper.Create([
        new(null, "Incorrect email address or password")
      ], 401, "ACCESS_DENIED");

    if (user.Status != UserStatus.Active)
      ThrowBadStatusException(user.Status);

    OnSuccessfulLogin(user, options);

    return await userService.SaveAsync(user, true, token);
  }

  /// <summary>
  /// Login the user as current identity against given JWT claims
  /// </summary>
  /// <param name="dto">The login claims object</param>
  /// <param name="options">Additional login options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User?> LoginWithClaimAsync(
    UserLoginClaimDto dto, LoginOptions? options = null, CancellationToken token = default
  ) {
    var user = await userService.GetByAuthKeyAsync(dto.Jti, token);

    // The missing user, which means the auth-key is no longer valid
    if (user == null) {
      throw ValidationHelper.Create([
        new(null, "Token may invalidated or user not found")
      ], 401, "TOKEN_INVALIDATED");
    }

    // Either a user is unverified or insufficient status
    if (user.Status != UserStatus.Active)
      ThrowBadStatusException(user.Status);

    // Forcefully logged-out user globally
    if (user.Metadata.Security.TokenInvalidate is true) {
      throw ValidationHelper.Create([
        new(null, "Token has been disabled, or revoked. Please generate a new one")
      ], 403, "ACCESS_REVOKED");
    }

    // Role mismatched. Tempered JWT "role" claim?
    if (user.Role != dto.Role) {
      throw ValidationHelper.Create([
        new(null, "Ineligible authorization role")
      ], 403, "INELIGIBLE_ROLE");
    }

    OnSuccessfulLogin(user, options);

    return await userService.SaveAsync(user, true, token);
  }

  /// <summary>
  /// Creates a user
  /// </summary>
  /// <param name="user">The user instance</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public Task Logout(User user, CancellationToken token = default) {
    throw new NotImplementedException();
  }

  /// <summary>
  /// User account status verification excluding "active"
  /// </summary>
  /// <param name="status">The status to verify</param>
  /// <exception cref="ValidationException">Throws when abnormal status detected</exception>
  public static void ThrowBadStatusException(UserStatus status) {
    if (status == UserStatus.Inactive) {
      throw ValidationHelper.Create([
        new("Status", "Cannot signed in due to the pending account verification.")
      ], 401, "VERIFICATION_PENDING");
    }

    if (status is UserStatus.Blocked or UserStatus.Deleted or UserStatus.Disabled) {
      throw ValidationHelper.Create([
        new("Status", $"Your account has been ${status.ToString().ToLower()}")
      ], 401, "ACCESS_REVOKED");
    }
  }

  /// <summary>
  /// A callback method which will be invoked when a user successfully authenticated
  /// </summary>
  /// <param name="user">The user object</param>
  /// <param name="options">The logged in options</param>
  private void OnSuccessfulLogin(User user, LoginOptions? options = null) {
    user.Metadata.Security.TokenInvalidate = false;
    user.Metadata.LoggedInHistory.SuccessCount += 1;
    user.Metadata.LoggedInHistory.LastIp = options?.IpAddress ?? null;
    user.Metadata.LoggedInHistory.LastDate = DateTime.UtcNow;
  }
}