// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Minimal.Endpoints.User;

public static class UserEndpointExtensions {
  public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app) {
    app.MapMeDetails();
    app.MapChangePassword();
    return app;
  }
}