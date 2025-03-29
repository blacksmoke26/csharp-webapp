// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Asp.Versioning;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Movies.Api;
using Movies.Application.Domain.Extensions;
using Movies.Contracts.Responses.Identity;

namespace Movies.Minimal.Endpoints.Auth;

public static class LoginEndpoint {
  public const string Name = "LoginAuth";

  public static IEndpointRouteBuilder MapLoginAuth(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.Auth.Login, async (
        UserLoginCredentialPayload body,
        IdentityService idService, AuthService authService, HttpContext context,
        CancellationToken token) => {
        var user = await idService.LoginAsync(body, new() {
          IpAddress = context.Connection.RemoteIpAddress?.ToString()
        }, token);

        ErrorHelper.ThrowIfNull(user,
          "Authenticate failed due to the unknown reason", 400, "AUTH_FAILED");

        return TypedResults.Ok(
          ResponseHelper.SuccessWithData(new UserAuthenticateResponse {
            Auth = authService.GenerateToken(user),
            User = user.ToLoggedInDetails()
          })
        );
      })
      .WithName(Name)
      .WithSummary("Login")
      .WithDescription("Account authentication using credentials")
      .WithTags("Auth")
      .Produces<SuccessResponse<UserAuthenticateResponse>>()
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity)
      .WithVersioning(ApiVersions.V10)
      /*.IsDeprecated("Please use the <code>loginUser</code> endpoint instead")*/;

    return app;
  }
}