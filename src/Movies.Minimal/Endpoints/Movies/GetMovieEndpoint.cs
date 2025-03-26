// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using CaseConverter;
using Movies.Api;
using Movies.Api.Core.Extensions;

namespace Movies.Minimal.Endpoints.Movies;

public static class GetMovieEndpoint {
  public const string Name = "GetMovie";

  public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app) {
    app.MapGet(ApiEndpoints.Movies.Get,
      async (string idOrSlug,
        MovieService movieService, HttpContext context, CancellationToken token) => {
        var movie = await movieService.GetBySlugOrPkAsync(idOrSlug, token);

        ErrorHelper.ThrowIfNull(
          movie, "This movie is no longer available", ErrorCodes.NotFound);

        // Note: With the abnormal status, only owner user can access this object. 
        ErrorHelper.ThrowWhenTrue(
          !context.GetIdentity().CheckSameId(context.GetIdOrNull(), true)
          && Enum.Parse<MovieStatus>(movie.Status.ToPascalCase()) != MovieStatus.Published,
          "This movie is no longer available or disabled by the owner", ErrorCodes.Unavailable
        );

        return TypedResults.Ok(ResponseHelper.SuccessWithData(movie));
      });

    return app;
  }
}