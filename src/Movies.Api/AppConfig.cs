// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api;

public struct JwtConfiguration {
  public required string Key { get; init; }
  public required string Issuer { get; init; }
  public required string Audience { get; init; }
  public required int ExpirationInHours { get; init; }
}

public class AppConfig(IConfiguration config) {
  /// <summary>
  /// Get JWT specific configuration
  /// <see cref="JwtConfiguration"/>
  /// </summary>
  public JwtConfiguration GetJwtConfig() {
    return new JwtConfiguration {
      Key = config["Jwt:Key"] ?? string.Empty,
      Issuer = config["Jwt:Issuer"] ?? string.Empty,
      Audience = config["Jwt:Audience"] ?? string.Empty,
      ExpirationInHours = int.Parse(config["Jwt:ExpirationInHours"] ?? "8"),
    };
  }
}