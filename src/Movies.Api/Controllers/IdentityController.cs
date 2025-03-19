// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Responses.Identity;

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
[Produces("application/json")]
[SwaggerTag("Signup, login, logout and other identity related actions")]
public class IdentityController(
  UserService userService,
  IdentityService idService,
  AuthService authService) : ControllerBase {
  /// <summary>
  /// Log in user against the credentials
  /// </summary>
  /// <param name="body">The user credentials</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to authenticate</exception>
  [HttpPost(ApiEndpoints.Identity.Login), AllowAnonymous]
  [SwaggerResponse(201, "The user was authenticated with credentials",
    typeof(SuccessResponse<UserAuthenticateResponse>))]
  [SwaggerResponse(400, "When failed to authenticated the user", typeof(ValidationFailureResponse))]
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
  /// Returns the authenticated user information
  /// </summary>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Identity.Me)]
  [SwaggerResponse(201, "The authenticated user details", typeof(SuccessResponse<UserMeResponse>))]
  [SwaggerResponse(401, "When user is not authenticated")]
  public Task<IActionResult> Me() {
    var user = HttpContext.GetIdentity().User;
    return Task.FromResult<IActionResult>(
      Ok(ResponseHelper.SuccessWithData(user.ToMeDetails()))
    );
  }

  /// <summary>
  /// Logouts the authenticated user
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Identity.Logout)]
  [SwaggerResponse(201, "Logouts the authenticated user", typeof(SuccessOnlyResponse))]
  [SwaggerResponse(401, "When user is not authenticated")]
  public async Task<IActionResult> Logout(CancellationToken token) {
    await idService.Logout(HttpContext.GetIdentity().User, token);
    return Ok(ResponseHelper.SuccessOnly());
  }
  
  /// <summary>
  /// Registers a new user account
  /// </summary>
  /// <param name="body">The signup payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to create user account</exception>
  [HttpPost(ApiEndpoints.Identity.Signup), AllowAnonymous]
  [SwaggerResponse(201, "The user account was created", typeof(SuccessWithMessageResponse))]
  [SwaggerResponse(400, "When failed to create user account", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> Signup(
    [FromBody, SwaggerRequestBody(Required = true)]
    UserSignupPayload body, CancellationToken token) {
    // creates a user account
    await userService.CreateAsync(new() {
      Email = body.Email,
      Password = body.Password,
      FirstName = body.FirstName,
      LastName = body.LastName
    }, token);

    return StatusCode(201, ResponseHelper.SuccessWithMessage(
      "You account has been created. Please check your inbox for verification email"));
  }
}