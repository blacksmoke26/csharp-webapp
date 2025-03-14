// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Domain.Model;

public record UserCreateModel {
  /// <summary>
  /// First name of the user
  /// </summary>
  public required string FirstName { get; init; }

  /// <summary>
  /// Last name of the user
  /// </summary>
  public required string LastName { get; init; }

  /// <summary>
  /// The unique email address
  /// </summary>
  public required string Email { get; init; }

  /// <summary>
  /// The password email address
  /// </summary>
  public required string Password { get; init; }

  /// <summary>
  /// The role of user
  /// </summary>
  public string Role { get; set; } = UserRole.User;

  /// <summary>
  /// The user account status
  /// </summary>
  public UserStatus Status { get; set; } = UserStatus.Inactive;

  /// <summary>
  /// Additional metadata store along with the user
  /// </summary>
  public UserMetadata? Metadata { get; set; }
}