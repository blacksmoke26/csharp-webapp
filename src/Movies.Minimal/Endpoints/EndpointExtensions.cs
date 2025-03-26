// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Minimal.Endpoints.Auth;
using Movies.Minimal.Endpoints.Identity;
using Movies.Minimal.Endpoints.Movies;
using Movies.Minimal.Endpoints.Ratings;
using Movies.Minimal.Endpoints.User;

namespace Movies.Minimal.Endpoints;

public static class EndpointExtensions {
  public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app) {
    app.MapAuthEndpoints();
    app.MapUserEndpoints();

    app.MapMovieEndpoints();
    app.MapRatingEndpoints();
    app.MapIdentityEndpoints();

    return app;
  }
}