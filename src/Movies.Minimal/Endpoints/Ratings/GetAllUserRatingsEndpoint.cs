// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using FluentValidation;
using Movies.Api;
using Movies.Api.Core.Extensions;
using Movies.Api.Domain.Query.Validators;
using Movies.Application.Core.Extensions;
using Movies.Contracts.Requests.Query;

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
      .RequireAuthorization(AuthPolicies.AuthPolicy);

    return app;
  }
}