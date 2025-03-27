// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Movies;

public static class DeleteMovieEndpoint {
  public const string Name = "DeleteMovie";

  public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app) {
    app.MapDelete(ApiEndpoints.Movies.Delete, async (
        [Description("The Movie ID")]
        long id,
        MovieService movieService, HttpContext context, CancellationToken token) => {
        var isFound = await movieService.ExistsAsync(x
          => x.UserId == context.GetId() && x.Id == id, token);

        ErrorHelper.ThrowWhenFalse(isFound, ErrorCodes.NotFound);

        var isFailed = await movieService.DeleteAsync(x
          => x.UserId == context.GetId() && x.Id == id, token) == 0;

        ErrorHelper.ThrowWhenTrue(isFailed,
          "An error occurred while deleting the movie", ErrorCodes.ProcessFailed);

        return TypedResults.Ok(ResponseHelper.SuccessOnly());
      })
      .WithName(Name)
      .WithSummary("Delete")
      .WithDescription("Deletes a movie")
      .WithTags("Movies")
      .RequireAuthorization(AuthPolicies.AdminPolicy)
      .Produces<SuccessResponse<SuccessOnlyResponse>>()
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<OperationFailureResponse>(StatusCodes.Status404NotFound);

    return app;
  }
}