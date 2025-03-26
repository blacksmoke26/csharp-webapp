﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using FluentValidation;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Api.Domain.Body.Validators.User;
using Movies.Contracts.Requests.Payload.User;

namespace Movies.Minimal.Endpoints.User;

public static class ChangePasswordEndpoint {
  public const string Name = "ChangePassword";

  public static IEndpointRouteBuilder MapChangePassword(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.User.ChangePassword, async (
      ChangePasswordPayload body,
      UserService userService, HttpContext context,
      ChangePasswordValidator changePasswordValidator,
      CancellationToken token) => {
      await changePasswordValidator.ValidateAndThrowAsync(body, token);
      
      var isChanged = await userService.ChangePassword(
        context.GetIdentity().User, body.NewPassword, token);
      ErrorHelper.ThrowWhenFalse(isChanged, 
        "Failed to change the password", ErrorCodes.ProcessFailed);
      
      return TypedResults.Ok(ResponseHelper.SuccessOnly());
    }).RequireAuthorization(AuthPolicies.AuthPolicy);

    return app;
  }
}