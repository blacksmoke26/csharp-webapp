// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController(IMovieRepository movieRepository) : ControllerBase {
  /// <summary>
  /// Create a new movie
  /// </summary>
  /// <param name="request">The request body</param>
  /// <returns>The created movie object</returns>
  [HttpPost(ApiEndpoints.Movies.Create)]
  public async Task<IActionResult> Create([FromBody] CreateMovieRequest request) {
    var movie = request.MapToMovie();
    await movieRepository.CreateAsync(movie);

    return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
  }

  /// <summary>
  /// Fetch the movie by its id or slug
  /// </summary>
  /// <param name="idOrSlug">Movie ID or Slug</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.Get)]
  public async Task<IActionResult> Get([FromRoute] string idOrSlug) {
    var movie = long.TryParse(idOrSlug, out var id)
      ? await movieRepository.GetByIdAsync(id)
      : await movieRepository.GetBySlugAsync(idOrSlug);

    return movie is null
      ? NotFound()
      : Ok(movie.MapToResponse());
  }

  /// <summary>
  /// Fetch all movies
  /// </summary>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  public async Task<IActionResult> GetAll() {
    var movies = await movieRepository.GetAllAsync();
    return Ok(movies.MapToResponse());
  }

  /// <summary>
  /// Update the movie by its id and details
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="request">Values to update</param>
  /// <returns>The movie response object</returns>
  [HttpPut(ApiEndpoints.Movies.Update)]
  public async Task<IActionResult> Update([FromRoute] long id, [FromBody] UpdateMovieRequest request) {
    var movie = request.MapToMovie(id);
    var update = await movieRepository.UpdateAsync(movie);

    return !update
      ? NotFound()
      : Ok(movie.MapToResponse());
  }

  /// <summary>
  /// Deletes the movie by its id
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <returns>The response</returns>
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  public async Task<IActionResult> Delete([FromRoute] long id) {
    var deleted = await movieRepository.DeleteByIdAsync(id);

    return !deleted
      ? NotFound()
      : Ok();
  }
}