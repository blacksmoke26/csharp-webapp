// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Api.Domain.Query.Validators;
using Movies.Application.Core.Extensions;
using Movies.Application.Domain.Filters;
using Movies.Contracts.Requests.Query;
using Movies.Contracts.Responses.Movies;

namespace Movies.Minimal.Endpoints.Movies;

public static class GetAllMoviesEndpoint {
  public const string Name = "GetAllMovies";

  public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app) {
    app.MapGet(ApiEndpoints.Movies.GetAll, async (
        [AsParameters]
        MoviesGetAllQuery query,
        MoviesGetAllQueryValidator allQueryValidator,
        HttpContext context,
        MovieService movieService, CancellationToken token
      ) => {
        await allQueryValidator.ValidateAndThrowAsync(query, token);
        var userId = context.GetIdOrNull();

        var paginated = await movieService.GetPaginatedAsync(
          MovieFilters.GetAllQuery(query, userId),
          query.GetPageOptions(), userId, token);

        return TypedResults.Ok(ResponseHelper.SuccessWithPaginated(paginated));
      })
      .WithName(Name)
      .WithSummary("Get All")
      .WithDescription("Fetch the list of movies <i>(using filters and sort order)</i>")
      .WithTags("Movies")
      .WithVersioning(ApiVersions.V10)
      .Produces<PaginatedResult<MovieResponse>>()
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}