// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using FluentValidation;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Api.Domain.Query.Validators;
using Movies.Application.Core.Extensions;
using Movies.Application.Domain.Filters;
using Movies.Contracts.Requests.Query;

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
      .WithName(Name);

    return app;
  }
}