// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Api.Domain.Body.Validators.User;
using Movies.Contracts.Requests.Payload.User;
using Movies.Contracts.Responses.Identity;

namespace Movies.Api.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V10)]
[Authorize(AuthPolicies.AuthPolicy)]
[Produces("application/json")]
[SwaggerTag("Authenticated user area")]
[SwaggerResponse(401, "When user is not authenticated")]
public class UserController(
  UserService userService,
  ChangePasswordValidator changePasswordValidator
) : ControllerBase {
  /// <summary>Returns the authenticated user information</summary>
  /// <returns>The HTTP response</returns>
  [HttpGet(ApiEndpoints.User.Me)]
  [SwaggerResponse(200, "The account information", typeof(SuccessResponse<UserMeResponse>))]
  [SwaggerResponse(401, "Unauthorized access")]
  public Task<IActionResult> Me() {
    var meDetails = HttpContext.GetIdentity().User.ToMeDetails();
    return Task.FromResult<IActionResult>(
      Ok(ResponseHelper.SuccessWithData(meDetails))
    );
  }

  /// <summary>Change account password</summary>
  /// <returns>The HTTP response</returns>
  [HttpPost(ApiEndpoints.User.ChangePassword)]
  [SwaggerResponse(200, "Password changed", typeof(SuccessOnlyResponse))]
  [SwaggerResponse(400, "Failed to change password", typeof(ValidationFailureResponse))]
  [SwaggerResponse(401, "Unauthorized access")]
  public async Task<IActionResult> ChangePassword(
    [FromBody, SwaggerRequestBody(Required = true)]
    ChangePasswordPayload body,
    CancellationToken token) {
    await changePasswordValidator.ValidateAndThrowAsync(body, token);
    var isChanged = await userService.ChangePassword(HttpContext.GetIdentity().User, body.NewPassword, token);
    ErrorHelper.ThrowWhenFalse(isChanged, "Failed to change the password", ErrorCodes.ProcessFailed);
    return Ok(ResponseHelper.SuccessOnly());
  }
}