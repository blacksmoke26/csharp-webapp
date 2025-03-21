// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Services;

public struct LoginOptions {
  public string? IpAddress { get; init; }
}

public class IdentityService(
  UserService userService,
  IValidator<UserLoginCredentialPayload> loginValidator
) : ServiceBase {
  /// <summary>HTTPContext items identity key name</summary>
  public const string IdentityKey = "%UserIdentity%";

  /// <summary>Logins the user by email and password</summary>
  /// <param name="input">The user DTO object</param>
  /// <param name="options">Additional login options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User?> LoginAsync(
    UserLoginCredentialPayload input, LoginOptions? options = null,
    CancellationToken token = default
  ) {
    await loginValidator.ValidateAndThrowAsync(input, token);

    var user = await userService.GetByEmailAsync(input.Email, token);

    ErrorHelper.ThrowWhenTrue(user == null || !user.ValidatePassword(input.Password),
      "Incorrect email address or password", ErrorCodes.AccessDenied);

    if (user.Status != UserStatus.Active)
      ThrowBadStatusException(user.Status);

    OnSuccessfulLogin(user, options);

    return await userService.SaveAsync(user, true, token);
  }

  /// <summary>Login the user as current identity against given JWT claims</summary>
  /// <param name="payload">The login claims object</param>
  /// <param name="options">Additional login options</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User?> LoginWithClaimAsync(
    UserLoginClaimPayload payload, LoginOptions? options = null, CancellationToken token = default
  ) {
    var user = await userService.GetByAuthKeyAsync(payload.Jti, token);

    // The missing user, which means the auth-key is no longer valid
    ErrorHelper.ThrowIfNull(user,
      "Token may invalidated or user not found", 401, "TOKEN_INVALIDATED");

    // Either a user is unverified or insufficient status
    if (user.Status != UserStatus.Active)
      ThrowBadStatusException(user.Status);

    // Forcefully logged-out user globally
    ErrorHelper.ThrowWhenTrue(user.Metadata.Security.TokenInvalidate is true,
      "The token is either disabled or revoked. Please sign in again.",
      ErrorCodes.AccessRevoked);

    // Role mismatched. Tempered JWT "role" claim?
    ErrorHelper.ThrowWhenTrue(user.Role != payload.Role,
      "Ineligible authorization role", 403, "INELIGIBLE_ROLE");

    OnSuccessfulLogin(user, options);

    return await userService.SaveAsync(user, true, token);
  }

  /// <summary>Creates a user</summary>
  /// <param name="user">The user instance</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public Task Logout(User user, CancellationToken token = default) {
    user.SetTokenInvalidateState(true);
    return userService.SaveAsync(user, true, token);
  }

  /// <summary>User account status verification excluding "active"</summary>
  /// <param name="status">The status to verify</param>
  /// <exception cref="ValidationException">Throws when abnormal status detected</exception>
  private static void ThrowBadStatusException(UserStatus status) {
    // Pending account
    ErrorHelper.ThrowWhenTrue(status == UserStatus.Inactive,
      "Cannot signed in due to the pending account verification.",
      401, "VERIFICATION_PENDING");

    // Abnormal account
    ErrorHelper.ThrowWhenTrue(status is UserStatus.Blocked or UserStatus.Deleted or UserStatus.Disabled,
      $"Your account has been ${status.ToString().ToLower()}",
      ErrorCodes.AccessRevoked);
  }

  /// <summary>A callback method which will be invoked when a user successfully authenticated</summary>
  /// <param name="user">The user object</param>
  /// <param name="options">The logged in options</param>
  private static void OnSuccessfulLogin(User user, LoginOptions? options = null) {
    user.SetTokenInvalidateState(false);
    user.Metadata.LoggedInHistory.OnLogin(options?.IpAddress);
  }
}