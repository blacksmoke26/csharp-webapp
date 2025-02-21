// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api.Services;
using Movies.Application.Models;

namespace Movies.Api.Controllers;

[ApiController]
public class TokenController : ControllerBase {
  /// <summary>
  /// Generates a token against the specified user
  /// </summary>
  /// <returns>Response with created JWT</returns>
  [HttpPost(ApiEndpoints.Token.Create)]
  public async Task<IActionResult> Create(AuthService authService) {
    await Task.Yield();

    User user = new() {
      Name = "Member User",
      Role = UserRole.User,
      Email = "member@example.com",
    };

    User adminUser = new() {
      Name = "Admin User",
      Role = UserRole.Admin,
      Email = "admin@example.com",
    };

    return Ok(new {
      Data = new {
        User = new {
          Token = authService.GenerateToken(user),
        },
        Admin = new {
          Token = authService.GenerateToken(adminUser),
        }
      }
    });
  }
}