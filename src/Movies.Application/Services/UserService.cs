// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Application.Core.Bases;
using Movies.Application.Core.Interfaces;
using Movies.Application.Domain.Model;
using Movies.Application.Helpers;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class UserService(
  UserRepository userRepo,
  IValidator<UserCreateModel> createValidator
) : ServiceBase, IServiceRepoInstance<UserRepository> {
  /// <inheritdoc/>
  public UserRepository GetRepo() => userRepo;
  
  /// <summary>
  /// Creates a new user account
  /// </summary>
  /// <param name="input">The user DTO object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created user / null when failed</returns>
  public async Task<User?> CreateAsync(
    UserCreateModel input, CancellationToken token = default
  ) {
    await createValidator.ValidateAndThrowAsync(input, token);

    var user = await CreateUserAsync(input, token);

    if (user is null) {
      throw ValidationHelper.Create([
        new() {
          ErrorMessage = "Failed to create account because there was error while processing the request.",
          ErrorCode = "PROCESS_FAILED"
        }
      ], 422);
    }

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
  /// Fetch the user by Email address
  /// </summary>
  /// <param name="email">The email address</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.Email == email, token);

  /// <summary>
  /// Fetch the user by Authorization Key
  /// </summary>
  /// <param name="authKey">The authorization Key</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByAuthKeyAsync(string authKey, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.AuthKey == authKey, token);

  /// <summary>
  /// Fetch the user by Password reset token
  /// </summary>
  /// <param name="resetToken">The password reset token</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  public Task<User?> GetByPasswordResetTokenAsync(string resetToken, CancellationToken token = default)
    => userRepo.GetOneAsync(x => x.PasswordResetToken == resetToken, token);

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

    user.SetPassword(dto.Password);

    userRepo.GetDataSet().Add(user);

    return await userRepo.GetDbContext().SaveChangesAsync(token) > 0 ? user : null;
  }

  /// <summary>
  /// Regenerates the authentication key
  /// </summary>
  /// <param name="user">The user</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The updated object, otherwise null on failed</returns>
  public Task<User?> RefreshAuthKeyAndSaveAsync(User user, CancellationToken token = default) {
    user.GenerateAuthKey();
    return SaveAsync(user, false, token);
  }

  /// <summary>
  /// Writes the user object changes into a database
  /// </summary>
  /// <param name="user">The change user object</param>
  /// <param name="jsonbChanged">When you make changes in JSONB columns, set this true to save changes as well</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The user instance if updated successfully, Null upon failed</returns>
  public async Task<User?> SaveAsync(
    User user, bool jsonbChanged = true, CancellationToken token = default) {
    userRepo.GetDataSet().Add(user);

    if (jsonbChanged) {
      // !Note: You must have this like to detected JSONB object changes
      userRepo.GetDataSet().Update(user).Property(x => x.Metadata).IsModified = true;
    }

    return await userRepo.GetDbContext().SaveChangesAsync(token) > 0 ? user : null;
  }
}