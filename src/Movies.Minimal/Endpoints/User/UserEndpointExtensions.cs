// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Minimal.Endpoints.User;

public static class UserEndpointExtensions {
  public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app) {
    app.MapMeDetails();
    app.MapChangePassword();
    return app;
  }
}