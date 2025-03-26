// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Contracts.Responses.Ratings;

namespace Movies.Application.Services;

public class RatingService(
  RatingRepository ratingRepo,
  MovieService movieService,
  IValidator<RatingCreateModel> createValidator)
  : ServiceBase {
  /// <summary>Rates the movie against the movie id by the user</summary>
  /// <param name="input">The input object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the rating was a success or failure</returns>
  public async Task<bool> RateMovieAsync(
    RatingCreateModel input, CancellationToken token = default) {
    await createValidator.ValidateAndThrowAsync(input, token);

    // Ensure that movie exists, otherwise the constraints exception thrown 
    var isFound = await movieService.ExistsAsync(x
      => x.Id == input.MovieId && x.Status != MovieStatus.Published, token);

    ErrorHelper.ThrowWhenFalse(isFound, "Movie not found", ErrorCodes.NotFound);

    var record = await ratingRepo.GetOneAsync(x
      => x.UserId == input.UserId && x.MovieId == input.MovieId, token);

    var model = record ?? new Rating();

    model.UserId = input.UserId;
    model.MovieId = input.MovieId;
    model.Score = input.Score;
    model.Feedback = input.Feedback;

    if (record is null)
      ratingRepo.GetDataSet().Add(model);
    else
      ratingRepo.GetDataSet().Update(model);

    return await ratingRepo.GetDbContext().SaveChangesAsync(token) > 0;
  }

  /// <summary>Deletes move rating against the user and the movie id</summary>
  /// <param name="movieId">The movie ID</param>
  /// <param name="userId">The user ID</param>
  /// <param name="token">The cancellation token</param>
  /// <exception cref="FluentValidation.ValidationException">If no rating was found to delete</exception>>
  /// <returns>Whatever the rating is deleted or not</returns>
  public async Task<bool> DeleteRatingAsync(long movieId, long userId, CancellationToken token) {
    return await ratingRepo.DeleteAsync(x
      => x.MovieId == movieId && x.UserId == userId, token) > 0;
  }

  /// <summary>Format the rating object response (Select LINQ expression)</summary>
  /// <param name="includeMovie">Include movie that associated with each rating</param>
  /// <returns>The query select expression</returns>
  private Expression<Func<Rating, RatingResponse>> PrepareResponse(bool includeMovie = false) {
    return x => new RatingResponse {
      Id = x.Id,
      MovieId = x.MovieId,
      Score = x.Score,
      Feedback = x.Feedback,
      CreatedAt = x.CreatedAt,
      UpdatedAt = x.UpdatedAt,
      Movie = includeMovie
        ? new MovieRatingResponse {
          Title = x.Movie.Title,
          YearOfRelease = x.Movie.YearOfRelease,
          Slug = x.Movie.Slug
        }
        : null,
    };
  }

  /// <summary>Fetches a single record from a database</summary>
  /// <param name="query">The query to filter/sort results</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetch record</returns>
  /// <exception cref="FluentValidation.ValidationException">If no record was found</exception>>
  public async Task<RatingResponse> GetOneAsync(
    Func<IQueryable<Rating>, IQueryable<Rating>>? query,
    CancellationToken token = default) {
    var record = await ratingRepo.GetOneAsync(
      q => query != null ? query.Invoke(q) : q,
      PrepareResponse(),
      token
    );

    ErrorHelper.ThrowIfNull(record, "Rating not found", ErrorCodes.NotFound);

    return record;
  }

  /// <summary>Fetches the multiple records from a database</summary>
  /// <param name="query">The query to filter/sort results</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>List of fetched records</returns>
  public Task<List<RatingResponse>> GetManyAsync(
    Func<IQueryable<Rating>, IQueryable<Rating>>? query,
    CancellationToken token = default) {
    return ratingRepo.GetManyAsync(
      q => query?.Invoke(q) ?? q,
      PrepareResponse(),
      token
    );
  }

  /// <summary>Fetches the multiple paginated ratings</summary>
  /// <param name="queryable">A callback function to perform a query on a current sequence.</param>
  /// <param name="options">The pagination options</param>
  /// <param name="includeMovie">Include movie that associated with each rating</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  public Task<PaginatedList<RatingResponse>> GetPaginatedAsync(
    Func<IQueryable<Rating>, IQueryable<Rating>>? queryable,
    PaginatorOptions options, bool includeMovie = false, CancellationToken token = default) {
    return ratingRepo.GetPaginatedAsync(
      queryable, PrepareResponse(includeMovie), options, token
    );
  }
}