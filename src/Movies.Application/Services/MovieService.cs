// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// See: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager

using Movies.Application.Core.Interfaces;
using Movies.Contracts.Responses.Movies;
using Movies.Contracts.Responses.Ratings;

namespace Movies.Application.Services;

public class MovieService(
  MovieRepository movieRepo,
  IValidator<MovieCreateModel> createValidator,
  IValidator<MovieUpdateModel> updateValidator)
  : ServiceBase, IServiceRepoInstance<MovieRepository> {
  /// <inheritdoc/>
  public MovieRepository GetRepo() => movieRepo;

  /// <summary>
  /// Creates a movie
  /// </summary>
  /// <param name="input">The input object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the movie is created or not</returns>
  public async Task<Movie?> CreateAsync(
    MovieCreateModel input, CancellationToken token = default) {
    await createValidator.ValidateAndThrowAsync(input, token);

    Movie movie = new() {
      UserId = input.UserId,
      Title = input.Title,
      YearOfRelease = input.YearOfRelease
    };
    
    var dbContext = movieRepo.GetDbContext();

    await using var transaction = await dbContext.BeginTransactionAsync(token);

    try {
      movieRepo.GetDataSet().Add(movie);
      await dbContext.SaveChangesAsync(token);

      // create new genres
      dbContext.Genres.AddRange(
        input.Genres.Select(name => new Genre {
          MovieId = movie.Id,
          Name = name
        })
      );

      await dbContext.SaveChangesAsync(token);
      await transaction.CommitAsync(token);
    }
    catch (Exception e) {
      Console.WriteLine(e);
      return null;
    }

    return movie;
  }

  /// <summary>
  /// Updates a movie
  /// </summary>
  /// <param name="input">The input data object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the movie is updated or not</returns>
  /// <exception cref="ValidationException">When the inputs validation failed</exception>
  public async Task<Movie?> UpdateAsync(
    MovieUpdateModel input, CancellationToken token = default) {
    await updateValidator.ValidateAndThrowAsync(input, token);

    var movie = await movieRepo.GetOneAsync(x => x.UserId == input.UserId && x.Id == input.Id, token);

    if (movie is null)
      throw ErrorHelper.CustomError("Movie not found", ErrorCodes.NotFound);

    var newSlug = movie.GenerateSlug(input.Title, input.YearOfRelease);
    var sameMovieExists = !newSlug.Equals(movie.Slug) &&
                          await movieRepo.ExistsAsync(x => x.Slug == newSlug, token);

    if (sameMovieExists) {
      throw ErrorHelper.CustomError(
        "Movie with the same name and year of release is already exist.", ErrorCodes.DuplicateEntry);
    }

    var dbContext = movieRepo.GetDbContext();

    await using var transaction = await dbContext.Database.BeginTransactionAsync(token);

    try {
      // remove existing genres
      await dbContext.Genres.Where(x => x.MovieId == movie.Id).ExecuteDeleteAsync(token);

      dbContext.Movies.Update(movie);
      await dbContext.SaveChangesAsync(token);

      foreach (var name in input.Genres) {
        dbContext.Genres.Add(new() {
          MovieId = movie.Id,
          Name = name
        });
      }

      await dbContext.SaveChangesAsync(token);
      await transaction.CommitAsync(token);

      return movie;
    }
    catch (Exception e) {
      Console.WriteLine(e);
      return null;
    }
  }

  /// <summary>
  /// Fetch the movie by the primary key
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<MovieResponse> GetByPkAsync(long id, CancellationToken token = default) {
    return GetOneAsync(null, q 
      => q.AsNoTracking().Where(x => x.Id == id), token);
  }

  /// <summary>
  /// Fetch the movie by the slug
  /// </summary>
  /// <param name="slug">Movie slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<MovieResponse> GetBySlugAsync(string slug, CancellationToken token = default) {
    return GetOneAsync(null, q 
      => q.AsNoTracking().Where(x => x.Slug == slug), token);
  }

  /// <summary>
  /// Fetch the movie by the given owner user id and movie id
  /// </summary>
  /// <param name="userId">The user id</param>
  /// <param name="movieId">The movie id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<MovieResponse> GetByUserIdAndPkAsync(
    long userId, long movieId, CancellationToken token = default) {
    return GetOneAsync(userId, q
      => q.Where(x => x.UserId == userId && x.Id == movieId), token);
  }

  /// <summary>
  /// Format the movie object response (Select LINQ expression)
  /// </summary>
  /// <param name="userId">The auth user ID</param>
  /// <returns>The query select expression</returns>
  private Expression<Func<Movie, MovieResponse>> PrepareMovieResponse(long? userId) {
    return x => new MovieResponse {
      Id = x.Id,
      UserId = x.UserId,
      Title = x.Title,
      YearOfRelease = x.YearOfRelease,
      Rating = (float?)x.Ratings.Average(z => z.Score),
      UserRating = x.Ratings
        .Where(r => r.UserId == userId)
        .Select(z => z.Score).SingleOrDefault(),
      Genres = x.Genres
        .Select(z => new GenreResponse { Name = z.Name })
        .OrderBy(g => g.Name)
        .ToList(),
      Ratings = x.Ratings.Select(z => new RatingResponse() {
          UserId = z.UserId,
          Score = z.Score,
          Feedback = z.Feedback,
          CreatedAt = z.CreatedAt,
          UpdatedAt = z.UpdatedAt,
        }).OrderByDescending(r => r.CreatedAt)
        .ToList(),
      Slug = x.Slug,
      Status = x.Status.ToString().ToLower(),
      CreatedAt = x.CreatedAt,
      UpdatedAt = x.UpdatedAt
    };
  }

  /// <inheritdoc cref="RepositoryBase{TModel}.GetOneAsync(System.Linq.Expressions.Expression{System.Func{TModel,bool}},System.Threading.CancellationToken)"/>
  public async Task<MovieResponse> GetOneAsync(
    long? userId, Func<IQueryable<Movie>, IQueryable<Movie>>? query,
    CancellationToken token = default) {
    var record = await movieRepo.GetOneAsync(
      q => query != null ? query.Invoke(q) : q,
      PrepareMovieResponse(userId),
      token
    );

    ErrorHelper.ThrowIfNull(record, "Movie not found", ErrorCodes.NotFound);

    return record!;
  }

  /// <summary>
  /// Fetches the multiple records from database
  /// </summary>
  /// <param name="query">The query to filter/sort results</param>
  /// <param name="userId">Authenticated user id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>List of fetched movie responses</returns>
  public Task<List<MovieResponse>> GetManyAsync(
    Func<IQueryable<Movie>, IQueryable<Movie>>? query,
    long? userId = null, CancellationToken token = default) {
    return movieRepo.GetManyAsync(
      q => query != null ? query.Invoke(q) : q,
      PrepareMovieResponse(userId),
      token
    );
  }

  /// <summary>
  /// Removes the movie object based on the specified condition
  /// </summary>
  /// <param name="condition">The where condition to filter records</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Count of deleted record</returns>
  public Task<int> DeleteAsync(
    Expression<Func<Movie, bool>> condition, CancellationToken token = default) {
    return movieRepo.DeleteAsync(condition, token);
  }
  
  /// <summary>
  /// Verifies the record exists based on a specified condition
  /// </summary>
  /// <param name="condition">The where condition to filter records</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the record exists</returns>
  public Task<bool> ExistsAsync(
    Expression<Func<Movie, bool>> condition, CancellationToken token = default) {
    return movieRepo.ExistsAsync(condition, token);
  }
}