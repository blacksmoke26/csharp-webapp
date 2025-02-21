// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api.Mapping;
using Movies.Api.Services;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController(IMovieService movieService) : ControllerBase {
  /// <summary>
  /// Create a new movie
  /// </summary>
  /// <param name="request">The request body</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created movie object</returns>
  [Authorize(Roles = UserRoles.Admin)]
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMovieRequest request,
    CancellationToken token) {
    var movie = request.MapToMovie();

    await movieService.CreateAsync(movie, token);

    return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
  }

  /// <summary>
  /// Fetch the movie by its id or slug
  /// </summary>
  /// <param name="idOrSlug">Movie ID or Slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token) {
    var movie = long.TryParse(idOrSlug, out var id)
      ? await movieService.GetByIdAsync(id, token)
      : await movieService.GetBySlugAsync(idOrSlug, token);

    return movie is null
      ? NotFound()
      : Ok(movie.MapToResponse());
  }

  /// <summary>
  /// Fetch all movies
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll(CancellationToken token) {
    var movies = await movieService.GetAllAsync(token);
    return Ok(movies.MapToResponse());
  }

  /// <summary>
  /// Update the movie by its id and details
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="request">Values to update</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [Authorize(Roles = UserRoles.Admin)]
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateMovieRequest request,
    CancellationToken token) {
    var movie = request.MapToMovie(id);
    var updatedMovie = await movieService.UpdateAsync(movie, token);

    return updatedMovie is null
      ? NotFound()
      : Ok(updatedMovie.MapToResponse());
  }

  /// <summary>
  /// Deletes the movie by its id
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The response</returns>
  [Authorize(Roles = UserRoles.Admin)]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken token) {
    var deleted = await movieService.DeleteByIdAsync(id, token);

    return !deleted
      ? NotFound()
      : Ok();
  }
}