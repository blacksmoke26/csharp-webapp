// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Minimal.Endpoints.Ratings;

public static class RatingEndpointExtensions {
  public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app) {
    app.MapRateMovie();
    app.MapDeleteRating();
    app.MapGetAllUserRatings();
    return app;
  }
}

  
