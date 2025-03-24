// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Seeders;

public static class UserSeeder {
  /// <summary>
  /// Seed users data into the database
  /// </summary>
  /// <param name="context">The database context</param>
  /// <param name="token">The cancellation token</param>
  public static async Task InitializeAsync(DbContext context, CancellationToken token) {
    await InsertFakeUsersAsync(context, token);
  }

  /// <summary>
  /// Seed fake users data into the database
  /// </summary>
  /// <param name="context">The database context</param>
  /// <param name="token">The cancellation token</param>
  private static async Task InsertFakeUsersAsync(DbContext context, CancellationToken token) {
    var userContext = context.Set<User>();
    
    if (await userContext.CountAsync(b => b.Email == "admin@example.com", token) is 0) {
      User user = new() {
        FirstName = "Admin",
        LastName = "User",
        Email = "admin@example.com",
        Role = UserRole.Admin,
        Status = UserStatus.Active,
      };
      user.SetPassword("Password@123");
      userContext.Add(user);
    }
    
    if (await userContext.CountAsync(b => b.Email == "user@example.com", token) is 0) {
      User user = new() {
        FirstName = "Ordinary",
        LastName = "User",
        Email = "user@example.com",
        Role = UserRole.User,
        Status = UserStatus.Active,
      };
      user.SetPassword("Password@123");
      userContext.Add(user);
    }

    await context.SaveChangesAsync(token);
  }
}