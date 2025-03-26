﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Application.Domain.Extensions;

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
      .RequireAuthorization(AuthPolicies.AuthPolicy);

    return app;
  }
}