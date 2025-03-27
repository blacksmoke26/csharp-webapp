// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Minimal.Endpoints.Auth;

public static class AuthEndpointExtensions {
  public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app) {
    app.MapLoginAuth();
    app.MapLogoutAuth();
    return app;
  }
}