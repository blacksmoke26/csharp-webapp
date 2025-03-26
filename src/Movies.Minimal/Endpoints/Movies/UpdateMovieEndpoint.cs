// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Movies;

public static class UpdateMovieEndpoint {
  public const string Name = "UpdateMovie";

  public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app) {
    app.MapPut(ApiEndpoints.Movies.Update,
      async (long id, MovieCreatePayload body,
        HttpContext context,
        MovieService movieService, CancellationToken token
      ) => {
        var movie = await movieService.UpdateAsync(new() {
          Id = id,
          UserId = context.GetId(),
          Title = body.Title,
          YearOfRelease = body.YearOfRelease,
          Genres = body.Genres
        }, token);

        ErrorHelper.ThrowIfNull(movie,
          "An error occurred while updating the movie", ErrorCodes.ProcessFailed);

        return TypedResults.Ok(ResponseHelper.SuccessWithData(movie));
      }).RequireAuthorization(AuthPolicies.AuthPolicy);

    return app;
  }
}