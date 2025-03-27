// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Ratings;

public static class RateMovieEndpoint {
  public const string Name = "RateMovie";

  public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app) {
    app.MapPut(ApiEndpoints.Movies.Rating, async (
        [Description("The Movie ID")]
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
      .WithName(Name)
      .WithSummary("Rate Movie")
      .WithDescription("Rates a single movie")
      .WithTags("Ratings")
      .RequireAuthorization()
      .Produces<SuccessOnlyResponse>()
      .Produces(StatusCodes.Status401Unauthorized)
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);
    return app;
  }
}