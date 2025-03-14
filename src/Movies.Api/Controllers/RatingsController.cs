// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Contracts.Requests.Dto;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingsController(
  RatingService ratingService,
  UserIdentity identity
) : ControllerBase {
  /// <summary>
  /// Rate the movie by providing the score and an optional feedback
  /// </summary>
  /// <param name="id">The movie ID</param>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns></returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to</exception>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPut(ApiEndpoints.Movies.Rate)]
  public async Task<IActionResult> RateMovie(
    [FromRoute]
    long id, [FromBody] MovieRatingDto body, CancellationToken token
  ) {
    var isRated = await ratingService.RateMovieAsync(new() {
      UserId = identity.GetId(),
      MovieId = id,
      Score = body.Rating,
      Feedback = body.Feedback,
    }, token);
    
    ErrorHelper.ThrowWhenFalse(
      isRated, "An error occurred while rating the movie", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessOnly());
  }
}