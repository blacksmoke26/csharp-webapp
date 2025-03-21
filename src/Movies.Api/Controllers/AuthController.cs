// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Identity;

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
[Produces("application/json")]
[SwaggerTag("User authentication operations")]
public class AuthController(
  IdentityService idService,
  AuthService authService) : ControllerBase {
  /// <summary>
  /// Log in with credentials
  /// </summary>
  /// <param name="body">The user credentials</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to authenticate</exception>
  [HttpPost(ApiEndpoints.Auth.Login), AllowAnonymous]
  [SwaggerResponse(200, "Authentication successfully",
    typeof(SuccessResponse<UserAuthenticateResponse>))]
  [SwaggerResponse(400, "Authentication failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(422, "Validation errors", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> Login(
    [FromBody, SwaggerRequestBody(Required = true)]
    UserLoginCredentialPayload body,
    CancellationToken token
  ) {
    var user = await idService.LoginAsync(body, new() {
      IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
    }, token);

    ErrorHelper.ThrowIfNull(user,
      "Authenticate failed due to the unknown reason", 400, "AUTH_FAILED");

    return Ok(
      ResponseHelper.SuccessWithData(new UserAuthenticateResponse {
        Auth = authService.GenerateToken(user),
        User = user.ToLoggedInDetails()
      })
    );
  }

  /// <summary>
  /// Logouts the authenticated user
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPost(ApiEndpoints.Auth.Logout)]
  [SwaggerResponse(200, "Logouts the authenticated user", typeof(SuccessOnlyResponse))]
  [SwaggerResponse(401, "Unauthorized access")]
  public async Task<IActionResult> Logout(CancellationToken token) {
    await idService.Logout(HttpContext.GetIdentity().User, token);
    return Ok(ResponseHelper.SuccessOnly());
  }
}