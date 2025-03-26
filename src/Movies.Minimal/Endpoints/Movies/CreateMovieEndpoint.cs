// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Movies;

public static class CreateMovieEndpoint {
  public const string Name = "CreateMovie";

  public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.Movies.Create,
      async (MovieCreatePayload body,
        HttpContext context,
        MovieService movieService, CancellationToken token
      ) => {
        var movie = await movieService.CreateAsync(new() {
          UserId = context.GetId(),
          Title = body.Title,
          YearOfRelease = body.YearOfRelease,
          Genres = body.Genres
        }, token);

        ErrorHelper.ThrowIfNull(movie,
          "An error occurred while creating the movie", ErrorCodes.ProcessFailed);

        return TypedResults.Ok(ResponseHelper.SuccessWithData(movie));
      }).RequireAuthorization(AuthPolicies.AuthPolicy);

    return app;
  }
}