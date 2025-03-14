﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Api.Controllers;

[ApiController]
public class IdentityController(
  UserService userService,
  IdentityService idService,
  AuthService authService) : ControllerBase {
  /// <summary>
  /// Registers a new user account
  /// </summary>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="ValidationException">
  /// When failed to create user account</exception>
  [HttpPost(ApiEndpoints.Identity.Signup)]
  public async Task<IActionResult> Signup(
    [FromBody]
    UserSignupDto body, CancellationToken token) {
    // creates a user account
    await userService.CreateAsync(new() {
      Email = body.Email,
      Password = body.Password,
      FirstName = body.FirstName,
      LastName = body.LastName
    }, token);

    return StatusCode(201, ResponseHelper.SuccessWithData(new {
      Message = "You account has been created. Please check your inbox for verification email"
    }));
  }

  /// <summary>
  /// Log in user against the credentials (email address and password)
  /// </summary>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">When failed to authenticate</exception>
  [HttpPost(ApiEndpoints.Identity.Login)]
  public async Task<IActionResult> Login(
    [FromBody]
    UserLoginCredentialDto body,
    CancellationToken token
  ) {
    var user = await idService.LoginAsync(body, new() {
      IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
    }, token);

    if (user is null)
      throw ErrorHelper.CustomError(
        "Authenticate failed due to the unknown reason", 400, "AUTH_FAILED");

    return Ok(
      ResponseHelper.SuccessWithData(new {
        Auth = authService.GenerateToken(user),
        User = new {
          user.FullName,
          user.FirstName,
          user.LastName,
          user.Email,
          user.Role
        }
      })
    );
  }

  /// <summary>
  /// Returns the authenticated user details
  /// </summary>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Identity.Me)]
  public Task<IActionResult> Me() {
    var user = HttpContext.GetIdentity().User;
    return Task.FromResult<IActionResult>(
      Ok(ResponseHelper.SuccessWithData(new {
        user.Id,
        user.FullName,
        user.FirstName,
        user.LastName,
        user.Email,
        user.Role,
        Status = user.Status.ToString().ToLower(),
        user.CreatedAt,
      }))
    );
  }

  /// <summary>
  /// Logouts the current user
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Identity.Logout)]
  public async Task<IActionResult> Logout(CancellationToken token) {
    await idService.Logout(HttpContext.GetIdentity().User, token);
    return Ok(ResponseHelper.SuccessOnly());
  }
}