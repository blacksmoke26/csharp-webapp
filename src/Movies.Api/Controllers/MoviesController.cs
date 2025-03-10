// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Requests.Dto;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController(
  MovieService movieService,
  UserIdentity identity
) : ControllerBase {
  /// <summary>
  /// Fetch the movie by its id or slug
  /// </summary>
  /// <param name="idOrSlug">Movie ID or Slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get(
    [FromRoute] string idOrSlug, CancellationToken token) {
    var movie = long.TryParse(idOrSlug, out var id)
      ? await movieService.GetByPkAsync(id, token)
      : await movieService.GetBySlugAsync(idOrSlug, token);

    // Note: With the abnormal status, only owner user can access this object. 
    return !identity.CheckSameId(movie.UserId) && movie.Status != MovieStatus.Published
      ? throw ValidationHelper.Create([
        new() {
          ErrorMessage = "This movie is no longer available or disabled by the owner"
        }
      ], 410, "NOT_AVAILABLE")
      : Ok(new SuccessResponse(movie));
  }

  /// <summary>
  /// Fetch all movies
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(CancellationToken token) {
    // TODO: Implement filters/pagination to limit the no. of records, otherwise **boom**
    var movies = await movieService.GetAllAsync(token: token);
    return Ok(new SuccessResponse(movies));
  }

  /// <summary>
  /// Creates a movie object
  /// </summary>
  /// <param name="body">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created movie object</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create(
    [FromBody]
    MovieCreateDto body,
    CancellationToken token
  ) {
    var movie = await movieService.CreateAsync(new() {
      UserId = identity.GetId(),
      Title = body.Title,
      YearOfRelease = body.YearOfRelease,
      Genres = body.Genres
    }, token);

    return movie is null
      ? throw ValidationHelper.Create([
        new() {
          ErrorMessage = "An error occurred while creating the movie"
        }
      ], 400, "PROCESS_FAILED")
      : Ok(new SuccessResponse(movie));
  }

  /// <summary>
  /// Update the movie details against movie id
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="body">Information to update</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update(
    [FromRoute]
    long id, [FromBody] MovieUpdateDto body, CancellationToken token
  ) {
    var movie = await movieService.UpdateAsync(new() {
      Id = id,
      UserId = identity.GetId(),
      Title = body.Title,
      YearOfRelease = body.YearOfRelease,
      Genres = body.Genres
    }, token);

    return movie is null
      ? throw ValidationHelper.Create([
        new() {
          ErrorMessage = "An error occurred while updating the movie"
        }
      ], 400, "PROCESS_FAILED")
      : Ok(new SuccessResponse(movie));
  }

  /// <summary>
  /// Deletes the movie by its id
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The response</returns>
  [Authorize(AuthPolicies.AdminPolicy)]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete(
    [FromRoute]
    long id, CancellationToken token
  ) {
    if (!await movieService.ExistsAsync(x
          => x.UserId == identity.GetId() && x.Id == id, token))
      return NotFound();

    return await movieService.DeleteAsync(x
      => x.UserId == identity.GetId() && x.Id == id, token) == 0
      ? throw ValidationHelper.Create([
        new() {
          ErrorMessage = "An error occurred while deleting the movie"
        }
      ], 400, "PROCESS_FAILED")
      : Ok(new SuccessOnlyResponse());
  }
}