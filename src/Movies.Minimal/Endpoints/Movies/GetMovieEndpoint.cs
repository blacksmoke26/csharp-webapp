// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using CaseConverter;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Contracts.Responses.Movies;

namespace Movies.Minimal.Endpoints.Movies;

public static class GetMovieEndpoint {
  public const string Name = "GetMovie";

  public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app) {
    app.MapGet(ApiEndpoints.Movies.Get, async (
        [Description("The Movie ID or a Slug")]
        string idOrSlug,
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
      })
      .WithName(Name)
      .WithSummary("Get One")
      .WithDescription("Fetch a single movie")
      .WithTags("Movies")
      .Produces<SuccessResponse<MovieResponse>>()
      .Produces<OperationFailureResponse>(StatusCodes.Status404NotFound)
      .Produces<OperationFailureResponse>(StatusCodes.Status410Gone)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}