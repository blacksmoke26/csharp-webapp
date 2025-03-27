// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Contracts.Responses.Movies;

namespace Movies.Minimal.Endpoints.Movies;

public static class UpdateMovieEndpoint {
  public const string Name = "UpdateMovie";

  public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app) {
    app.MapPut(ApiEndpoints.Movies.Update, async (
        [Description("The Movie ID")]
        long id, MovieCreatePayload body,
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
      })
      .WithName(Name)
      .WithSummary("Update")
      .WithDescription("Updates a single movie")
      .WithTags("Movies")
      .RequireAuthorization()
      .Produces<SuccessResponse<MovieResponse>>()
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status401Unauthorized)
      .Produces<OperationFailureResponse>(StatusCodes.Status404NotFound)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}