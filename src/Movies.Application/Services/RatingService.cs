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
  /// <summary>
  /// Rates the movie against the movie id by the user 
  /// </summary>
  /// <param name="input">The user id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the rating was a success or failure</returns>
  public async Task<bool> RateMovieAsync(
    RatingCreateModel input, CancellationToken token = default) {
    await createValidator.ValidateAndThrowAsync(input, token);

    // Ensure that movie exists, otherwise the constraints exception thrown 
    if (!await movieService.ExistsAsync(x
          => x.Id == input.MovieId && x.Status != MovieStatus.Published, token)) {
      throw ErrorHelper.CustomError("Movie not found", ErrorCodes.NotFound);
    }

    var record = await ratingRepo.GetOneAsync(x
      => x.UserId == input.UserId && x.MovieId == input.MovieId, token);

    var model = record ?? new Rating();

    model.UserId = input.UserId;
    model.MovieId = input.MovieId;
    model.Score = input.Score;
    model.Feedback = input.Feedback;

    ratingRepo.GetDataSet().Add(model);
    return await ratingRepo.GetDbContext().SaveChangesAsync(token) > 0;
  }

  /// <summary>
  /// Deletes move rating against the user and the movie id 
  /// </summary>
  /// <param name="movieId">The movie ID</param>
  /// <param name="userId">The user ID</param>
  /// <param name="token">The cancellation token</param>
  /// <exception cref="FluentValidation.ValidationException">If no rating was found to delete</exception>>
  /// <returns>Whatever the rating is deleted or not</returns>
  public async Task<bool> DeleteRatingAsync(long movieId, long userId, CancellationToken token) {
    return await ratingRepo.DeleteAsync(x
      => x.MovieId == movieId && x.UserId == userId, token) > 0;
  }
  
  /// <summary>
  /// Format the rating object response (Select LINQ expression)
  /// </summary>
  /// <returns>The query select expression</returns>
  private Expression<Func<Rating, RatingResponse>> PrepareResponse() {
    return x => new RatingResponse {
      Movie = new () {
        Id = x.MovieId,
        Title = x.Movie.Title,
        Slug = x.Movie.Slug,
      },
      Score = x.Score,
      Feedback = x.Feedback,
      CreatedAt = x.CreatedAt,
      UpdatedAt = x.UpdatedAt
    };
  }

  /// <summary>
  /// Fetches a single record from a database
  /// </summary>
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

    return record!;
  }

  /// <summary>
  /// Fetches the multiple records from a database
  /// </summary>
  /// <param name="query">The query to filter/sort results</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>List of fetched records</returns>
  public Task<List<RatingResponse>> GetManyAsync(
    Func<IQueryable<Rating>, IQueryable<Rating>>? query,
    CancellationToken token = default) {
    return ratingRepo.GetManyAsync(
      q => query != null ? query.Invoke(q) : q,
      PrepareResponse(),
      token
    );
  }
}