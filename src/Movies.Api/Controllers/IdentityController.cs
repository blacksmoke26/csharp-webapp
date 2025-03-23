// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.EntityFrameworkCore;
using Movies.Api.Domain.Body.Validators.Identity;
using Movies.Contracts.Requests.Payload.Identity;

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
[Produces("application/json")]
[SwaggerTag("Identity specific operations")]
[AllowAnonymous]
public class IdentityController(
  UserService userService,
  UserRepository userRepository,
  PasswordResetRequestPayloadValidator resetRequestPayloadValidator,
  PasswordResetValidator passwordResetValidator
) : ControllerBase {
  /// <summary>
  /// Create user account
  /// </summary>
  /// <param name="body">The request payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to create user account</exception>
  [HttpPost(ApiEndpoints.Identity.Signup)]
  [SwaggerResponse(201, "Account created", typeof(SuccessWithMessageResponse))]
  [SwaggerResponse(400, "Process failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(422, "Validation errors", typeof(ValidationFailureResponse))]
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

  /// <summary>
  /// Send request for reset password
  /// </summary>
  /// <param name="body">The request payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to create user account</exception>
  [HttpPost(ApiEndpoints.Identity.PasswordResetRequest)]
  [SwaggerResponse(200, "Request sent", typeof(SuccessWithMessageResponse))]
  [SwaggerResponse(404, "Email address not found", typeof(OperationFailureResponse))]
  [SwaggerResponse(403, "Access denied", typeof(OperationFailureResponse))]
  [SwaggerResponse(400, "Process failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(410, "Account unavailable", typeof(OperationFailureResponse))]
  [SwaggerResponse(422, "Validation errors", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> PasswordResetRequest(
    [FromBody, SwaggerRequestBody(Required = true)]
    PasswordResetRequestPayload body, CancellationToken token) {
    await resetRequestPayloadValidator.ValidateAndThrowAsync(body, token);

    var user = await userService.GetByEmailAsync(body.Email, token);

    ErrorHelper.ThrowIfNull(user,
      "The email address does not exist", ErrorCodes.NotFound);

    ErrorHelper.ThrowWhenTrue(user.Status == UserStatus.Inactive,
      "This account requires verification", ErrorCodes.Forbidden);

    ErrorHelper.ThrowWhenTrue(user.Status != UserStatus.Active,
      "This account has been deleted or disabled by the admin", ErrorCodes.Unavailable);

    ErrorHelper.ThrowWhenFalse(await userService.SendResetPasswordRequest(user, token),
      "Process failed due to the unknown error", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessOnly());
  }

  /// <summary>
  /// Reset password
  /// </summary>
  /// <param name="body">The request payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to create user account</exception>
  [HttpPost(ApiEndpoints.Identity.PasswordReset)]
  [SwaggerResponse(200, "Password reset", typeof(SuccessWithMessageResponse))]
  [SwaggerResponse(400, "Process failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(404, "Email address or reset code not found", typeof(OperationFailureResponse))]
  [SwaggerResponse(422, "Validation errors", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> PasswordReset(
    [FromBody, SwaggerRequestBody(Required = true)]
    PasswordResetPayload body, CancellationToken token) {
    await passwordResetValidator.ValidateAndThrowAsync(body, token);

    var user = await userRepository.GetOneAsync(q => q
      .AsNoTracking()
      .Where(x
        => x.Email == body.Email
           && x.Status == UserStatus.Active
           && x.Metadata.Password.ResetCode == body.ResetCode), token);

    ErrorHelper.ThrowIfNull(user,
      "Email address or reset code not found", ErrorCodes.NotFound);

    ErrorHelper.ThrowWhenTrue(user.Metadata.Password.IsResetCodeExpired(),
      "The reset code has been expired", ErrorCodes.BadRequest);

    ErrorHelper.ThrowWhenFalse(await userService.ResetPassword(user, body.NewPassword, token),
      "Process failed due to the unknown error", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessOnly());
  }
}