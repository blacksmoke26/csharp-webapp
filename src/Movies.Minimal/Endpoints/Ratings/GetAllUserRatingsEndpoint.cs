// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Api.Domain.Query.Validators;
using Movies.Application.Core.Extensions;
using Movies.Contracts.Requests.Query;
using Movies.Contracts.Responses.Ratings;

namespace Movies.Minimal.Endpoints.Ratings;

public static class ListUserRatingEndpoint {
  public const string Name = "GetAllUserRatings";

  public static IEndpointRouteBuilder MapGetAllUserRatings(this IEndpointRouteBuilder app) {
    app.MapGet(ApiEndpoints.Ratings.GetUserRatings, async (
        [AsParameters]
        RatingsGetAllQuery query,
        RatingService ratingService,
        RatingsGetAllQueryValidator allQueryValidator,
        HttpContext context,
        CancellationToken token
      ) => {
        await allQueryValidator.ValidateAndThrowAsync(query, token);

        var paginated = await ratingService.GetPaginatedAsync(x
            => x.Where(r => r.UserId == context.GetId()),
          query.GetPageOptions(), true, token);

        return TypedResults.Ok(ResponseHelper.SuccessWithPaginated(paginated));
      })
      .WithName(Name)
      .WithSummary("Get All")
      .WithDescription("Get all user ratings")
      .WithTags("Ratings")
      .WithAuthorization(AuthPolicies.AdminPolicy)
      .WithVersioning(ApiVersions.V10)
      .Produces<PaginatedResult<RatingResponse>>()
      .Produces(StatusCodes.Status401Unauthorized)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}