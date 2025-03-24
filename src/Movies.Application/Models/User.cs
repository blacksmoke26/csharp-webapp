﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models;

[Table("users")]
[Index("AuthKey", Name = "UNQ_users_auth_key", IsUnique = true)]
[Index("Email", Name = "UNQ_users_email", IsUnique = true)]
public class User : ModelBase {
  /// <summary>ID</summary>
  [Key] [Column("id")]
  public long Id { get; set; }

  /// <summary>Email Address</summary>
  [Column("email")] [StringLength(255)]
  public string Email { get; set; } = null!;

  /// <summary>Password</summary>
  [Column("password")] [StringLength(64)]
  public string Password { get; set; } = null!;

  /// <summary>Authorization Key</summary>
  [Column("auth_key")] [StringLength(64)]
  public string AuthKey { get; set; } = null!;

  /// <summary>Password Hash</summary>
  [Column("password_hash")] [StringLength(150)]
  public string PasswordHash { get; set; } = null!;

  /// <summary>First name</summary>
  [Column("first_name")] [StringLength(20)]
  public string FirstName { get; set; } = null!;

  /// <summary>Last name</summary>
  [Column("last_name")] [StringLength(20)]
  public string LastName { get; set; } = null!;

  /// <summary>Role</summary>
  [Column("role")] [StringLength(20)]
  public string Role { get; set; } = UserRole.User;

  /// <summary>Status</summary>
  [Column("status")]
  public UserStatus Status { get; set; }

  /// <summary>Metadata</summary>
  [Column("metadata", TypeName = "jsonb")]
  public UserMetadata Metadata { get; set; } = new();

  /// <summary>Created</summary>
  [Column("created_at", TypeName = "timestamp without time zone")]
  public DateTime? CreatedAt { get; set; }

  /// <summary>Updated</summary>
  [Column("updated_at", TypeName = "timestamp without time zone")]
  public DateTime? UpdatedAt { get; set; }

  /// <summary>List of movies associated with this user</summary>
  [InverseProperty("User")]
  public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

  /// <summary>List of ratings associated with this user</summary>
  [InverseProperty("User")]
  public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

  /// <summary>Returns the user's fullname</summary>
  [NotMapped]
  public virtual string FullName => string.Concat(FirstName.Trim(), " ", LastName.Trim());

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(EntityState state, CancellationToken token = default) {
    if (state is EntityState.Added) {
      this.GenerateAuthKey();
      CreatedAt = DateTime.UtcNow;
    }

    if (state is EntityState.Added or EntityState.Modified) {
      UpdatedAt = DateTime.UtcNow;
    }

    return Task.CompletedTask;
  }
}