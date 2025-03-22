// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Read more: https://medium.com/@serhiikokhan/jsonb-in-postgresql-with-ef-core-cc945f1aba2a

using Movies.Application.Core.Interfaces;

namespace Movies.Application.Repositories;

public enum UserStatus {
  Deleted = 0,
  Disabled = 1,
  Blocked = 2,
  Inactive = 3,
  Active = 10
}

public static class UserRole {
  /// <summary>
  /// Only allowed for `Admin` role
  /// </summary>
  public const string Admin = "admin";

  /// <summary>
  /// Only allowed for `User` role
  /// </summary>
  public const string User = "user";

  /// <summary>
  /// Returns the list of user roles
  /// </summary>
  public static string[] GetRoles() => [Admin, User];
}

public class UserRepository(DatabaseContext dbContext)
  : RepositoryBase<User>, IDbJsonbSaveChanges<User> {
  /// <inheritdoc/>
  public override DatabaseContext GetDbContext() => dbContext;

  /// <inheritdoc/>
  public override DbSet<User> GetDataSet() => dbContext.Users;

  /// <inheritdoc/>
  public Task<User?> SaveAsync(User entity, bool jsonbChanged = true, CancellationToken token = default) {
    if (jsonbChanged) {
      // !Note: You must have this like to detected JSONB object changes
      GetDataSet().Update(entity).Property(x => x.Metadata).IsModified = true;
    }

    return SaveAsync(entity, token);
  }
}