// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Core.Interfaces;

namespace Movies.Application.Services;

public class UserService(
  UserRepository userRepo,
  IValidator<UserCreateModel> createValidator
) : ServiceBase, IDbSaveChanges<User>, IDbJsonbSaveChanges<User> {
  /// <summary>
  /// Creates a new user account
  /// </summary>
  /// <param name="input">The user DTO object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User> CreateAsync(UserCreateModel input, CancellationToken token = default) {
    await createValidator.ValidateAndThrowAsync(input, token);

    var user = await CreateUserAsync(input, token);

    ErrorHelper.ThrowIfNull(user,
      "Failed to create account because there was an error while processing the request.",
      ErrorCodes.ProcessFailed);

    return user;
  }

  /// <summary>
  /// Fetch the user by Primary Key
  /// </summary>
  /// <param name="id">User ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByPkAsync(long id, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.Id == id, token);

  /// <summary>
  /// Fetch a single record against the given condition
  /// </summary>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetOne(
    Expression<Func<User, bool>> predicate, CancellationToken token = default)
    => userRepo.GetOneAsync(predicate, token);

  /// <summary>
  /// Fetch the user by Email address
  /// </summary>
  /// <param name="email">The email address</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.Email == email, token);

  /// <summary>
  /// Fetch the user by Email address and request reset code
  /// </summary>
  /// <param name="email">The email address</param>
  /// <param name="resetCode">The request reset code</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByEmailAndResetCodeAsync(string email, string resetCode, CancellationToken token = default) {
    return userRepo.GetOneAsync(q => q
      .AsNoTracking()
      .Where(x =>
        x.Email == email
        && x.Status == UserStatus.Active
        && x.Metadata.Password.ResetCode == resetCode), token);
  }

  /// <summary>
  /// Fetch the user by Authorization Key
  /// </summary>
  /// <param name="authKey">The authorization Key</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByAuthKeyAsync(string authKey, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.AuthKey == authKey, token);

  /// <summary>
  /// Verify that the given email address actually exists
  /// </summary>
  /// <param name="email">The email address</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True when existed, false otherwise</returns>
  public Task<bool> EmailExistsAsync(string email, CancellationToken token = default)
    => userRepo.ExistsAsync(x => x.Email == email, token);

  /// <summary>
  /// Creates a new user entity and write change on database
  /// </summary>
  /// <param name="dto">The user details</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The user instance if created successfully, Null upon failed</returns>
  private async Task<User?> CreateUserAsync(UserCreateModel dto, CancellationToken token = default) {
    var user = new User {
      Email = dto.Email.ToLower(),
      Password = dto.Password,
      FirstName = dto.FirstName,
      LastName = dto.LastName,
      Role = dto.Role,
      Status = dto.Status,
      Metadata = dto.Metadata ?? new()
    };

    user.Metadata.Activation.OnSignedUp();

    user.SetPassword(dto.Password);

    userRepo.GetDataSet().Add(user);

    return await userRepo.GetDbContext().SaveChangesAsync(token) > 0 ? user : null;
  }

  /// <summary>
  /// Regenerates the authentication key
  /// </summary>
  /// <param name="user">The user object</param>
  /// <param name="newPassword">The password to change</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the password changed or not</returns>
  public async Task<bool> ChangePassword(User user, string newPassword, CancellationToken token = default) {
    user.SetPassword(newPassword);
    user.Metadata.Password.OnUpdate();
    user.GenerateAuthKey();

    return await SaveAsync(user, true, token) != null;
  }

  /// <summary>
  /// Sends a request for a password reset
  /// </summary>
  /// <param name="user">The user object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the process was a success or failure</returns>
  public async Task<bool> SendResetPasswordRequest(User user, CancellationToken token = default) {
    user.Metadata.Password.OnResetRequest();
    return await SaveAsync(user, true, token) != null;
  }

  /// <summary>
  /// Changes the account password
  /// </summary>
  /// <param name="user">The user object</param>
  /// <param name="password">The password to set</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the process was a success or failure</returns>
  public async Task<bool> ResetPassword(User user, string password, CancellationToken token = default) {
    user.SetPassword(password);
    user.SetTokenInvalidateState(true);
    user.Metadata.Password.OnReset();
    return await SaveAsync(user, true, token) != null;
  }

  /// <inheritdoc/>
  public Task<User?> SaveAsync(User entity, CancellationToken token = default) {
    return userRepo.SaveAsync(entity, token);
  }

  /// <inheritdoc/>
  public Task<User?> SaveAsync(User entity, bool jsonbChanged = true, CancellationToken token = default) {
    return userRepo.SaveAsync(entity, jsonbChanged, token);
  }
}