// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Ratings;

public static class DeleteRatingEndpoint {
  public const string Name = "DeleteRating";

  public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app) {
    app.MapDelete(ApiEndpoints.Movies.DeleteRating, async (
        long movieId,
        RatingService ratingService, HttpContext context, CancellationToken token) => {
        ErrorHelper.ThrowWhenFalse(
          await ratingService.DeleteRatingAsync(movieId, context.GetId(), token),
          "No rating was found against the movie", ErrorCodes.NotFound);

        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .RequireAuthorization(AuthPolicies.AuthPolicy);
    return app;
  }
}