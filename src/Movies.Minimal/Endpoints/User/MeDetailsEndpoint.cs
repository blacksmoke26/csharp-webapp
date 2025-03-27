// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Application.Domain.Extensions;
using Movies.Contracts.Responses.Identity;

namespace Movies.Minimal.Endpoints.User;

public static class MeDetailsEndpoint {
  public const string Name = "MeDetails";

  public static IEndpointRouteBuilder MapMeDetails(this IEndpointRouteBuilder app) {
    app.MapGet(ApiEndpoints.User.Me, (
        HttpContext context) => {
        var meDetails = context.GetIdentity().User.ToMeDetails();
        return TypedResults.Ok(ResponseHelper.SuccessWithData(meDetails));
      })
      .WithName(Name)
      .WithSummary("Me")
      .WithDescription("Fetch the account information")
      .WithTags("User")
      .RequireAuthorization()
      .Produces<SuccessResponse<UserMeResponse>>()
      .Produces(StatusCodes.Status401Unauthorized);

    return app;
  }
}