// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Ratings;

public static class DeleteRatingEndpoint {
  public const string Name = "DeleteRating";

  public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app) {
    app.MapDelete(ApiEndpoints.Movies.DeleteRating, async (
        [Description("The Movie ID")]
        long movieId,
        RatingService ratingService, HttpContext context, CancellationToken token) => {
        ErrorHelper.ThrowWhenFalse(
          await ratingService.DeleteRatingAsync(movieId, context.GetId(), token),
          "No rating was found against the movie", ErrorCodes.NotFound);

        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .WithName(Name)
      .WithSummary("Delete")
      .WithDescription("Delete a movie rating")
      .WithTags("Ratings")
      .RequireAuthorization()
      .Produces<SuccessOnlyResponse>()
      .Produces(StatusCodes.Status401Unauthorized)
      .Produces<OperationFailureResponse>(StatusCodes.Status404NotFound);
    return app;
  }
}