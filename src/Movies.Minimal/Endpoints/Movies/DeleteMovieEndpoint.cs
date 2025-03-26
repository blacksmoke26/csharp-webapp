// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Movies;

public static class DeleteMovieEndpoint {
  public const string Name = "DeleteMovie";

  public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app) {
    app.MapDelete(ApiEndpoints.Movies.Delete, async (
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
      .RequireAuthorization(AuthPolicies.AdminPolicy);

    return app;
  }
}