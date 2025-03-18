// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://blog.jetbrains.com/dotnet/2024/06/26/how-where-conditions-work-in-entity-framework-core/

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
public class MoviesController(
  MovieService movieService,
  MoviesGetAllQueryValidator allQueryValidator
) : ControllerBase {
  /// <summary>
  /// Fetch the movie by its id or slug
  /// </summary>
  /// <param name="idOrSlug">Movie ID or Slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get(
    [FromRoute]
    string idOrSlug, CancellationToken token) {
    var movie = await movieService.GetBySlugOrPkAsync(idOrSlug, token);

    ErrorHelper.ThrowIfNull(
      movie, "This movie is no longer available or disabled by the owner", ErrorCodes.Unavailable);

    // Note: With the abnormal status, only owner user can access this object. 
    ErrorHelper.ThrowWhenTrue(
      !HttpContext.GetIdentity().CheckSameId(HttpContext.GetIdOrNull(), true)
      && Enum.Parse<MovieStatus>(movie.Status!) != MovieStatus.Published,
      "This movie is no longer available or disabled by the owner", ErrorCodes.Unavailable
    );

    return Ok(ResponseHelper.SuccessWithData(movie));
  }

  /// <summary>
  /// Fetch all movies
  /// </summary>
  /// <param name="query">The cancellation token</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(
    [FromQuery]
    MoviesGetAllQuery query, CancellationToken token
  ) {
    await allQueryValidator.ValidateAndThrowAsync(query, token);

    var userId = HttpContext.GetIdOrNull();

    var paginated = await movieService.GetPaginatedAsync(
      MovieFilters.GetAllQuery(query, userId),
      query.GetPageOptions(), userId, token);

    return Ok(ResponseHelper.SuccessWithPaginated(paginated.ToPaginatedResult()));
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
      UserId = HttpContext.GetId(),
      Title = body.Title,
      YearOfRelease = body.YearOfRelease,
      Genres = body.Genres
    }, token);

    ErrorHelper.ThrowIfNull(movie,
      "An error occurred while creating the movie", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessWithData(movie));
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
      UserId = HttpContext.GetId(),
      Title = body.Title,
      YearOfRelease = body.YearOfRelease,
      Genres = body.Genres
    }, token);

    ErrorHelper.ThrowIfNull(movie,
      "An error occurred while updating the movie", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessWithData(movie));
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
    var isFound = await movieService.ExistsAsync(x
      => x.UserId == HttpContext.GetId() && x.Id == id, token);

    ErrorHelper.ThrowWhenFalse(isFound, ErrorCodes.NotFound);

    var isFailed = await movieService.DeleteAsync(x
      => x.UserId == HttpContext.GetId() && x.Id == id, token) == 0;
    
    ErrorHelper.ThrowWhenTrue(isFailed,
      "An error occurred while deleting the movie", ErrorCodes.ProcessFailed);

    return Ok(ResponseHelper.SuccessOnly());
  }
}