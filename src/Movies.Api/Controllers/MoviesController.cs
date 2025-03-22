// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://blog.jetbrains.com/dotnet/2024/06/26/how-where-conditions-work-in-entity-framework-core/

using CaseConverter;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Core.Caching;
using Movies.Contracts.Responses.Movies;

namespace Movies.Api.Controllers;

[ApiVersion(ApiVersions.V10)]
[ApiController]
[Produces("application/json")]
[SwaggerTag("Manage and list movies")]
public class MoviesController(
  MovieService movieService,
  IOutputCacheStore cacheStore,
  MoviesGetAllQueryValidator allQueryValidator
) : ControllerBase {
  /// <summary>Fetch the movie ID or a Slug</summary>
  /// <param name="idOrSlug">The requested movie id or slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  //[Obsolete("Use the <code>GetAll</code> instead")]
  [HttpGet(ApiEndpoints.Movies.Get)]
  [OutputCache(PolicyName = CachePolicies.Movies.Tag)]
  //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept, Accept-Encoding, Accept-Language")]
  [SwaggerResponse(StatusCodes.Status200OK, "Movie details", typeof(SuccessResponse<MovieResponse>))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Not found", typeof(OperationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Validation errors", typeof(ValidationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status410Gone, "Unavailable", typeof(OperationFailureResponse))]
  public async Task<IActionResult> Get(
    [FromRoute]
    string idOrSlug, CancellationToken token) {
    var movie = await movieService.GetBySlugOrPkAsync(idOrSlug, token);

    ErrorHelper.ThrowIfNull(
      movie, "This movie is no longer available", ErrorCodes.NotFound);

    // Note: With the abnormal status, only owner user can access this object. 
    ErrorHelper.ThrowWhenTrue(
      !HttpContext.GetIdentity().CheckSameId(HttpContext.GetIdOrNull(), true)
      && Enum.Parse<MovieStatus>(movie.Status.ToPascalCase()) != MovieStatus.Published,
      "This movie is no longer available or disabled by the owner", ErrorCodes.Unavailable
    );

    return Ok(ResponseHelper.SuccessWithData(movie));
  }

  /// <summary>
  /// Fetch all movies using filters and sort order
  /// </summary>
  /// <param name="query">The query to filter/sort the list of movies</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [HttpGet(ApiEndpoints.Movies.GetAll)]
  [OutputCache(PolicyName = CachePolicies.Movies.Tag)]
  [SwaggerResponse(StatusCodes.Status200OK, "Movie List", typeof(PaginatedResult<MovieResponse>))]
  [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Validation errors", typeof(ValidationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Process failed", typeof(OperationFailureResponse))]
  public async Task<IActionResult> GetAll(
    [FromQuery]
    MoviesGetAllQuery query, CancellationToken token
  ) {
    await allQueryValidator.ValidateAndThrowAsync(query, token);

    var userId = HttpContext.GetIdOrNull();

    var paginated = await movieService.GetPaginatedAsync(
      MovieFilters.GetAllQuery(query, userId),
      query.GetPageOptions(), userId, token);

    return Ok(ResponseHelper.SuccessWithPaginated(paginated));
  }

  /// <summary>
  /// Creates a movie
  /// </summary>
  /// <param name="body">The updated movie payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The created movie object</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPost(ApiEndpoints.Movies.Create)]
  [SwaggerResponse(StatusCodes.Status200OK, "Movie created", typeof(SuccessResponse<MovieResponse>))]
  [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Process failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Validation errors", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> Create(
    [FromBody, SwaggerRequestBody(Required = true)]
    MovieCreatePayload body,
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

    await cacheStore.EvictByTagAsync(CachePolicies.Movies.Tag, token);

    return Ok(ResponseHelper.SuccessWithData(movie));
  }

  /// <summary>
  /// Update the movie details against movie id
  /// </summary>
  /// <param name="id">The requested movie id</param>
  /// <param name="body">The updated movie payload</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The movie response object</returns>
  [Authorize(AuthPolicies.AuthPolicy)]
  [HttpPut(ApiEndpoints.Movies.Update)]
  [SwaggerResponse(StatusCodes.Status200OK, "Movie updated", typeof(SuccessResponse<MovieResponse>))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Process failed", typeof(OperationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access")]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Not found", typeof(OperationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Validation errors", typeof(ValidationFailureResponse))]
  public async Task<IActionResult> Update(
    [FromRoute, SwaggerRequestBody(Required = true)]
    long id,
    [FromBody, SwaggerRequestBody(Required = true)]
    MovieUpdatePayload body,
    CancellationToken token
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

    await cacheStore.EvictByTagAsync(CachePolicies.Movies.Tag, token);

    return Ok(ResponseHelper.SuccessWithData(movie));
  }

  /// <summary>
  /// Deletes the movies
  /// </summary>
  /// <param name="id">The requested movie id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The HTTP response</returns>
  //[Obsolete("Method1 is deprecated, please use Method2 instead.", true)]
  [Authorize(AuthPolicies.AdminPolicy)]
  [HttpDelete(ApiEndpoints.Movies.Delete)]
  [SwaggerResponse(StatusCodes.Status200OK, "Movie deleted", typeof(SuccessResponse<SuccessOnlyResponse>))]
  [SwaggerResponse(StatusCodes.Status404NotFound, "Not found", typeof(OperationFailureResponse))]
  [SwaggerResponse(StatusCodes.Status400BadRequest, "Process failed", typeof(OperationFailureResponse))]
  public async Task<IActionResult> Delete(
    [FromRoute, SwaggerRequestBody(Required = true)]
    long id, CancellationToken token
  ) {
    var isFound = await movieService.ExistsAsync(x
      => x.UserId == HttpContext.GetId() && x.Id == id, token);

    ErrorHelper.ThrowWhenFalse(isFound, ErrorCodes.NotFound);

    var isFailed = await movieService.DeleteAsync(x
      => x.UserId == HttpContext.GetId() && x.Id == id, token) == 0;

    ErrorHelper.ThrowWhenTrue(isFailed,
      "An error occurred while deleting the movie", ErrorCodes.ProcessFailed);

    await cacheStore.EvictByTagAsync(CachePolicies.Movies.Tag, token);

    return Ok(ResponseHelper.SuccessOnly());
  }
}