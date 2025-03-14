// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Core.Interfaces;

namespace Movies.Application.Services;

public class RatingService(
  RatingRepository ratingRepo,
  MovieService movieService,
  IValidator<RatingCreateModel> createValidator)
  : ServiceBase, IServiceRepoInstance<RatingRepository> {
  /// <inheritdoc/>
  public RatingRepository GetRepo() => ratingRepo;

  /// <summary>
  /// Rates the movie
  /// </summary>
  /// <param name="input">The user id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the rating was a success or failure</returns>
  public async Task<bool> RateMovieAsync(
    RatingCreateModel input, CancellationToken token = default) {
    await createValidator.ValidateAsync(input, token);

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
}