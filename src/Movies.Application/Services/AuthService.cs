// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://www.telerik.com/blogs/asp-net-core-basics-authentication-authorization-jwt

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Movies.Application.Services;

public class AuthService(JwtConfiguration config) : ServiceBase {
  /// <summary>
  /// GeneratesJWT token against the given user
  /// </summary>
  /// <param name="user">The user model instance</param>
  /// <param name="options">Additional options to customize the token</param>
  /// <returns>The generated Json Web Token</returns>
  public TokenResult GenerateToken(User user, JwtOptions? options = null) {
    JwtSecurityTokenHandler tokenHandler = new();

    SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(config.Key));

    var issuedAt = DateTime.UtcNow;
    var expires = DateTime.UtcNow.AddHours(options?.ExpirationInHours ?? config.ExpirationInHours);

    SecurityTokenDescriptor tokenDescriptor = new() {
      Subject = GenerateClaims(user, options),
      Expires = expires,
      Issuer = options?.Issuer ?? config.Issuer,
      IssuedAt = issuedAt,
      Audience = options?.Audience ?? config.Audience,
      SigningCredentials = new(
        securityKey, SecurityAlgorithms.HmacSha384Signature
      )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return new() {
      Token = tokenHandler.WriteToken(token),
      IssuedAt = issuedAt,
      Expires = expires,
    };
  }

  /// <summary>
  /// Generate the token claims
  /// </summary>
  /// <param name="user">The user</param>
  /// <param name="options">Additional options to customize the token</param>
  /// <returns>The generated claims list</returns>
  private static ClaimsIdentity GenerateClaims(User user, JwtOptions? options = null) {
    ClaimsIdentity claims = new();

    claims.AddClaims([
      new Claim(JwtRegisteredClaimNames.Jti, user.AuthKey),
      new Claim(JwtRegisteredClaimNames.Sub, "auth"),
      new Claim(ClaimTypes.Role, user.Role),
    ]);

    if (options is null) return claims;
    foreach (var claim in options.Value.Claims)
      claims.AddClaim(claim);

    return claims;
  }
}

/// <summary>
/// AuthPolicies represents the JWT role policies
/// <p>Check <see cref="AuthService"/> class for usage.</p>
/// </summary>
public static class AuthPolicies {
  /// <summary>
  /// Applicable to `Admin` role
  /// </summary>
  public const string AdminPolicy = "AdminPolicy";

  /// <summary>
  /// Applicable to `User` role
  /// </summary>
  public const string UserPolicy = "UserPolicy";

  /// <summary>
  /// Applicable to signed-in users
  /// </summary>
  public const string AuthPolicy = "AuthPolicy";
}

public struct TokenResult {
  public required string Token { get; init; }
  public DateTime IssuedAt { get; init; }
  public DateTime Expires { get; init; }
}

/// <summary>
/// Additional JWT options to customize the token
/// </summary>
public struct JwtOptions {
  public string Issuer { get; init; }
  public string Audience { get; init; }
  public int ExpirationInHours { get; init; }
  public readonly IList<Claim> Claims => [];
}