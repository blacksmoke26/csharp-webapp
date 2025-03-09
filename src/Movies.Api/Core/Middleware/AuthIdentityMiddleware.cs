// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Movies.Application.Objects;

namespace Movies.Api.Core.Middleware;

/// <summary>
/// Server middleware to validate against auth-key, fetch user and set as current identity upon verified
/// </summary>
/// <param name="next">The middleware delegate</param>
public class AuthValidationMiddleware(
  RequestDelegate next,
  IdentityService idService,
  UserIdentity userIdentity
) {
  /// <summary>
  /// List of allowed login claims
  /// </summary>
  private string[] ValidLoginClaims { get; } = [
    ClaimTypes.Role,
    JwtRegisteredClaimNames.Jti
  ];

  public async Task InvokeAsync(HttpContext context) {
    var claims = context.User.Claims.ToArray()
      .Where(x => ValidLoginClaims.Contains(x.Type.ToString()))
      .Select(x => new KeyValuePair<string, string>(x.Type, x.Value))
      .ToDictionary();

    if (!claims.ContainsKey(JwtRegisteredClaimNames.Jti)
        || string.IsNullOrWhiteSpace(claims[JwtRegisteredClaimNames.Jti])) {
      // Skipping anonymous request
      await next(context);
      return;
    }

    // Fetch and validate user against claims
    var user = await idService.LoginWithClaimAsync(new() {
      Jti = claims[JwtRegisteredClaimNames.Jti],
      Role = claims[ClaimTypes.Role]
    }, new() {
      IpAddress = context.Connection.RemoteIpAddress?.ToString(),
    });

    if (user is null) {
      throw ValidationHelper.Create([
        new() {
          ErrorMessage = "Authenticate failed due to the unknown reason",
          ErrorCode = "AUTH_FAILED"
        }
      ]);
    }

    // Set user as authenticated identity
    userIdentity.SetIdentity(user);

    await next(context);
  }
}