// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Identity;

namespace Movies.Application.Domain.Extensions;

public static class UserModelExtensions {
  /// <summary>
  /// Encrypt the password and also generate the password hash
  /// </summary>
  /// <param name="user">The User model</param>
  /// <param name="password">The password to set</param>
  public static void SetPassword(this User user, string password) {
    var result = IdentityHelper.EncryptPassword(password);
    user.Password = result.Password;
    user.PasswordHash = result.PasswordHash;
  }

  /// <summary>
  /// Sets the token invalidation state
  /// </summary>
  /// <param name="user">The User model</param>
  /// <param name="state">The invalidate state to set</param>
  public static void SetTokenInvalidateState(this User user, bool state) =>
    user.Metadata.Security.TokenInvalidate = state;

  /// <summary>
  /// Validates the given password against the existing password
  /// </summary>
  /// <param name="user">The User model</param>
  /// <param name="password">The password to verify</param>
  /// <returns>True if password is correct, false otherwise</returns>
  public static bool ValidatePassword(this User user, string password) =>
    IdentityHelper.ValidatePassword(new EncryptedPasswordResult {
      Password = password,
      PasswordHash = user.PasswordHash,
    });

  /// <summary>
  /// Generates token authentication key
  /// </summary>
  /// <param name="user">The User model</param>
  public static void GenerateAuthKey(this User user)
    => user.AuthKey = IdentityHelper.GenerateAuthKey();

  /// <summary>
  /// Validates the given auth key
  /// </summary>
  /// <param name="user">The User model</param>
  /// <param name="authKey">The key to validate</param>
  /// <returns>True when the key is valid, False otherwise</returns>
  public static bool ValidateAuthKey(this User user, string authKey) => user.AuthKey == authKey;

  /// <summary>
  /// Map the current user to the <c>UserLoggedInDetails</c> object
  /// </summary>
  /// <param name="user">The User model</param>
  /// <returns>The mapped object</returns>
  public static UserAuthInfo ToLoggedInDetails(this User user) {
    return new UserAuthInfo {
      Fullname = user.FullName,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      Role = user.Role
    };
  }

  /// <summary>
  /// Map the current user to the <c>UserMeDetails</c> object
  /// </summary>
  /// <param name="user">The User model</param>
  /// <returns>The mapped object</returns>
  public static UserMeResponse ToMeDetails(this User user) {
    return new UserMeResponse {
      Id = user.Id,
      Fullname = user.FullName,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Email = user.Email,
      Role = user.Role,
      Status = user.Status.ToString().ToLower(),
      CreatedAt = user.CreatedAt,
    };
  }
}