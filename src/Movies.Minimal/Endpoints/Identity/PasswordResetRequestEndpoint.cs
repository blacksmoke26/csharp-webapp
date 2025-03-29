// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Api;
using Movies.Api.Domain.Body.Validators.Identity;
using Movies.Contracts.Requests.Payload.Identity;

namespace Movies.Minimal.Endpoints.Identity;

public static class PasswordResetRequestEndpoint {
  public const string Name = "PasswordResetRequestIdentity";

  public static IEndpointRouteBuilder MapPasswordResetRequestIdentity(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.Identity.PasswordResetRequest, async (
        PasswordResetRequestPayload body,
        PasswordResetRequestPayloadValidator resetRequestPayloadValidator,
        UserService userService,
        CancellationToken token
      ) => {
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

        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .WithName(Name)
      .WithSummary("Request password reset")
      .WithDescription("Sends a request for resetting password")
      .WithTags("Identity")
      .WithVersioning(ApiVersions.V10)
      .Produces<SuccessWithMessageResponse>()
      .Produces<OperationFailureResponse>(StatusCodes.Status404NotFound)
      .Produces<OperationFailureResponse>(StatusCodes.Status403Forbidden)
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<OperationFailureResponse>(StatusCodes.Status410Gone)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}