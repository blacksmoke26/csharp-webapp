// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Microsoft.Extensions.Configuration;

namespace Movies.Application.Config;

/// <summary>
/// AppConfiguration represents the application configuration retrieval
/// </summary>
/// <param name="config">Application configuration instance</param>
public class AppConfiguration(IConfiguration config) {
  /// <summary>
  /// Returns the IConfiguration instance
  /// </summary>
  /// <returns>The IConfiguration instance</returns>
  public IConfiguration GetConfig() => config;

  /// <summary>
  /// Get JWT specific configuration
  /// <see cref="JwtConfiguration"/>
  /// </summary>
  public JwtConfiguration JwtConfig()
    => config.GetRequiredSection("Jwt").Get<JwtConfiguration>();

  /// <summary>
  /// Retrieve the authentication configuration from application configuration
  /// <see cref="AuthenticationConfiguration"/>
  /// </summary>
  /// <returns>The authentication configuration</returns>
  public AuthenticationConfiguration AuthConfig()
    => config.GetRequiredSection("Authentication").Get<AuthenticationConfiguration>();

  /// <summary>
  /// Retrieve the database configuration from application configuration
  /// <see cref="DatabaseConfiguration"/>
  /// </summary>
  /// <returns>The database configuration</returns>
  public DatabaseConfiguration DbConfig()
    => config.GetRequiredSection("Database").Get<DatabaseConfiguration>();
}