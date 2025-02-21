// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/
// See: https://www.telerik.com/blogs/asp-net-core-basics-authentication-authorization-jwt

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Movies.Application.Models;

namespace Movies.Api.Services;

public static class UserRoles {
  public const string Admin = "admin";
  public const string User = "user";

  public static string FromUserRole(UserRole role) {
    return role switch {
      UserRole.Admin => Admin,
      UserRole.User => User,
      _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
    };
  }
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

public class AuthService(AppConfig appConfig) {
  private JwtConfiguration Config => appConfig.GetJwtConfig();

  /// <summary>
  /// Generate a JWT token against the given user
  /// </summary>
  /// <param name="user">The user</param>
  /// <param name="options">Additional options to customize the token</param>
  /// <returns>Generate JWT token</returns>
  public string GenerateToken(User user, JwtOptions? options = null) {
    JwtSecurityTokenHandler tokenHandler = new();

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Key));

    SecurityTokenDescriptor tokenDescriptor = new() {
      Subject = GenerateClaims(user, options),
      Expires = DateTime.UtcNow.AddHours(options?.ExpirationInHours ?? Config.ExpirationInHours),
      Issuer = options?.Issuer ?? Config.Issuer,
      Audience = options?.Audience ?? Config.Audience,
      SigningCredentials = new SigningCredentials(
        securityKey, SecurityAlgorithms.HmacSha384Signature
      )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  /// <summary>
  /// Generate the token claims
  /// </summary>
  /// <param name="user">The user</param>
  /// <param name="options">Additional options to customize the token</param>
  /// <returns>The generated claims list</returns>
  private static ClaimsIdentity GenerateClaims(User user, JwtOptions? options = null) {
    var claims = new ClaimsIdentity();
    claims.AddClaims([
      new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(ClaimTypes.Role, UserRoles.FromUserRole(user.Role)),
    ]);

    if (options is null) return claims;

    foreach (var claim in options.Value.Claims)
      claims.AddClaim(claim);

    return claims;
  }
}