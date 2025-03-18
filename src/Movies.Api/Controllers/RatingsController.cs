// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/


namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
public class RatingsController(
  RatingService ratingService
) : ControllerBase {
  /// <summary>
  /// Rate the movie by providing the score and an optional feedback
  /// </summary>
  /// <param name="movieId">The movie ID</param>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns></returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to rate a movie</exception>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPut(ApiEndpoints.Movies.Rating)]
  public async Task<IActionResult> RateMovie(
    [FromRoute]
    long movieId, [FromBody] MovieRatingDto body, CancellationToken token
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
  /// <param name="movieId">The movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  /// <exception cref="FluentValidation.ValidationException">When failed to delete the rating</exception>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpDelete(ApiEndpoints.Movies.DeleteRating)]
  public async Task<IActionResult> DeleteRating(
    [FromRoute]
    long movieId, CancellationToken token
  ) {
    ErrorHelper.ThrowWhenFalse(
      await ratingService.DeleteRatingAsync(movieId, HttpContext.GetId(), token), 
      "No rating was found against the movie", ErrorCodes.NotFound);

    return Ok(ResponseHelper.SuccessOnly());
  }
  
  /// <summary>
  /// Fetches the user ratings
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpGet(ApiEndpoints.Ratings.GetUserRatings)]
  public async Task<IActionResult> DeleteRating(CancellationToken token) {
    var records = await ratingService.GetManyAsync(x
      => x.Where(r => r.UserId == HttpContext.GetId()), token);
    return Ok(ResponseHelper.SuccessWithData(records));
  }
}