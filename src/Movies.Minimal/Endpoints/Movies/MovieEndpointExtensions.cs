// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Minimal.Endpoints.Movies;

public static class MovieEndpointExtensions {
  public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app) {
    app.MapGetMovie();
    app.MapGetAllMovies();
    app.MapCreateMovie();
    app.MapUpdateMovie();
    app.MapDeleteMovie();
    return app;
  }
}