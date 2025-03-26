// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Ratings;

public static class RateMovieEndpoint {
  public const string Name = "RateMovie";

  public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app) {
    app.MapPut(ApiEndpoints.Movies.Rating, async (
        long movieId,
        MovieRatingPayload body,
        HttpContext context,
        RatingService ratingService,
        CancellationToken token) => {
        var isRated = await ratingService.RateMovieAsync(new() {
          UserId = context.GetId(),
          MovieId = movieId,
          Score = body.Rating,
          Feedback = body.Feedback,
        }, token);

        ErrorHelper.ThrowWhenFalse(
          isRated, "An error occurred while rating the movie", ErrorCodes.ProcessFailed);
        
        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .RequireAuthorization(AuthPolicies.AuthPolicy);
    return app;
  }
}