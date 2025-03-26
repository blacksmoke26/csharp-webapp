// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Minimal.Endpoints.Identity;

public static class IdentityEndpointExtensions {
  public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app) {
    app.MapSignupIdentity();
    app.MapPasswordResetRequestIdentity();
    app.MapPasswordResetIdentity();
    return app;
  }
}