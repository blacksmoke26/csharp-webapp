// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Auth;

public static class LogoutEndpoint {
  public const string Name = "LogoutAuth";

  public static IEndpointRouteBuilder MapLogoutAuth(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.Auth.Logout, async (
        IdentityService idService, HttpContext context,
        CancellationToken token) => {
        await idService.Logout(context.GetIdentity().User, token);

        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .WithName(Name)
      .WithSummary("Logout")
      .WithDescription("Logouts the authenticated user")
      .WithTags("Auth")
      .WithAuthorization()
      .WithVersioning(ApiVersions.V10)
      .Produces<SuccessResponse<SuccessOnlyResponse>>();

    return app;
  }
}