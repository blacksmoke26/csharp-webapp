// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle
// Reference: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#swashbuckleaspnetcoreannotations

using Movies.Contracts.Responses.Ratings;

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
[Produces("application/json")]
[SwaggerTag("Manage and list user rating")]
public class RatingsController(
  RatingService ratingService,
  RatingsGetAllQueryValidator allQueryValidator
) : ControllerBase {
  /// <summary>
  /// Rate the movie by providing the score and an optional feedback
  /// </summary>
  /// <param name="movieId">The movie rating payload</param>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns></returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to rate a movie</exception>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPut(ApiEndpoints.Movies.Rating)]
  [SwaggerResponse(200, "Operation successful", typeof(SuccessOnlyResponse))]
  [SwaggerResponse(400, "Failed due to the validation errors", typeof(ValidationFailureResponse))]
  [SwaggerResponse(400, "Failed to rate a movie", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> RateMovie(
    [FromRoute, SwaggerRequestBody(Required = true)]
    long movieId,
    [FromBody, SwaggerRequestBody(Required = true)]
    MovieRatingPayload body,
    CancellationToken token
  ) {
    var isRated = await ratingService.RateMovieAsync(new() {
      UserId = HttpContext.GetId(),
      MovieId = movieId,
      Score = body.Rating,
      Feedback = body.Feedback,
    }, token);

    ErrorHelper.ThrowWhenFalse(
      isRated, "An error occurred while rating the movie", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessOnly());
  }

  /// <summary>
  /// Deletes the movie rating against movie and user id
  /// </summary>
  /// <param name="movieId">The movie rating payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to delete the rating</exception>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
  [SwaggerResponse(200, "Deletion successful", typeof(SuccessOnlyResponse))]
  [SwaggerResponse(404, "If no rating was found", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> DeleteRating(
    [FromRoute, SwaggerRequestBody(Required = true)]
    long movieId, CancellationToken token
  ) {
    ErrorHelper.ThrowWhenFalse(
      await ratingService.DeleteRatingAsync(movieId, HttpContext.GetId(), token),
      "No rating was found against the movie", ErrorCodes.NotFound);

    return Ok(ResponseHelper.SuccessOnly());
  }

  /// <summary>
  /// Get all authenticated user ratings
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <param name="query">The query to filter/sort the list of movies</param>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
  [SwaggerResponse(200, "The paginated list", typeof(PaginatedResult<RatingResponse>))]
  [SwaggerResponse(400, "If the query contains invalid values", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> GetAll(
    [FromQuery]
    RatingsGetAllQuery query,
    CancellationToken token) {
    await allQueryValidator.ValidateAndThrowAsync(query, token);

    var paginated = await ratingService.GetPaginatedAsync(x
        => x.Where(r => r.UserId == HttpContext.GetId()),
      query.GetPageOptions(), true, token);

    return Ok(ResponseHelper.SuccessWithPaginated(paginated));
  }
}