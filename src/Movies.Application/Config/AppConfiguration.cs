// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Microsoft.Extensions.Configuration;

namespace Movies.Application.Config;

public class AppConfiguration(IConfiguration config) {
  public IConfiguration Config { get; } = config;

  /// <summary>
  /// Get JWT specific configuration
  /// <see cref="JwtConfiguration"/>
  /// </summary>
  public JwtConfiguration JwtConfig() => new() {
    Key = Config["Jwt:Key"] ?? string.Empty,
    Issuer = Config["Jwt:Issuer"] ?? string.Empty,
    Audience = Config["Jwt:Audience"] ?? string.Empty,
    ExpirationInHours = int.Parse(Config["Jwt:ExpirationInHours"] ?? "8")
  };

  /// <summary>
  /// Get Authentication configuration
  /// <see cref="Authentication"/>
  /// </summary>
  public Authentication AuthConfig() => new() {
    ExpireTokenAfterLogout =
      bool.Parse(Config["Authentication:ExpireTokenAfterLogout"] ?? "false")
  };
}

public struct Authentication {
  public bool ExpireTokenAfterLogout { get; init; }
}

/// <summary>
/// JwtConfiguration presents the options required by JWT Auth service
/// <p>Check <see cref="AppConfiguration"/> class for usage.</p>
/// </summary>
public struct JwtConfiguration {
  /// <summary>Represents a symmetric security key.</summary>
  public required string Key { get; init; }

  /// <summary>identifies the principal that issued the JWT</summary>
  public required string Issuer { get; init; }

  /// <summary>Identifies the recipients that the JWT is intended for.</summary>
  public required string Audience { get; init; }

  /// <summary>The expiration time in hours  on or after which the JWT MUST NOT be accepted for processing</summary>
  public required int ExpirationInHours { get; init; }
}