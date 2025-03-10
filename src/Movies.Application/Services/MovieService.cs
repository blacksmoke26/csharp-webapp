// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using System.Linq.Expressions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Core.Bases;
using Movies.Application.Database;
using Movies.Application.Domain.Model;
using Movies.Application.Helpers;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(
  DatabaseContext dbContext,
  MovieRepository movieRepo,
  IValidator<MovieCreateModel> createValidator,
  IValidator<MovieUpdateModel> updateValidator): ServiceBase {
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

    await using var transaction = await dbContext.Database.BeginTransactionAsync(token);

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
  public async Task<Movie?> UpdateAsync(MovieUpdateModel input, CancellationToken token = default) {
    await updateValidator.ValidateAndThrowAsync(input, token);
    var movie = await GetByUserIdAndPkAsync(input.UserId, input.Id, token);

    await using var transaction = await dbContext.Database.BeginTransactionAsync(token);

    try {
      dbContext.Movies.Add(movie);
      await dbContext.SaveChangesAsync(token);

      // remove existing genres
      await dbContext.Genres.Where(x => x.MovieId == movie.Id).ExecuteDeleteAsync(token);

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
  /// Fetch the movie by the primary key
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<Movie> GetByPkAsync(long id, CancellationToken token = default) {
    return GetOneAsync(x => x.Id == id, token);
  }

  /// <summary>
  /// Fetch the movie by the slug
  /// </summary>
  /// <param name="slug">Movie slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<Movie> GetBySlugAsync(string slug, CancellationToken token = default) {
    return GetOneAsync(x => x.Slug == slug, token);
  }

  /// <summary>
  /// Fetch the movie by the given owner user id and movie id
  /// </summary>
  /// <param name="userId">The user id</param>
  /// <param name="movieId">The movie id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<Movie> GetByUserIdAndPkAsync(
    long userId, long movieId, CancellationToken token = default) {
    return GetOneAsync(x => x.UserId == userId && x.Id == movieId, token);
  }

  /// <inheritdoc cref="MovieRepository.GetOneAsync"/>
  public async Task<Movie> GetOneAsync(Expression<Func<Movie, bool>> whereFn, CancellationToken token = default) {
    var movie = await movieRepo.GetOneAsync(whereFn, token);

    if (movie is not null) return movie;

    throw ValidationHelper.Create([
      new() {
        ErrorMessage = "Movie not found"
      }
    ], 404, "NOT_FOUND");
  }

  /// <inheritdoc cref="MovieRepository.GetManyAsync"/>
  public Task<List<Movie>> GetAllAsync(Expression<Func<Movie, bool>>? whereFn = null, CancellationToken token = default) {
    return movieRepo.GetManyAsync(whereFn, token);
  }

  /// <inheritdoc cref="MovieRepository.DeleteAsync"/>
  public Task<int> DeleteAsync(Expression<Func<Movie, bool>> whereFn, CancellationToken token = default) {
    return movieRepo.DeleteAsync(whereFn, token);
  }

  /// <inheritdoc cref="MovieRepository.ExistsAsync"/>
  public Task<bool> ExistsAsync(Expression<Func<Movie, bool>> whereFn, CancellationToken token = default) {
    return movieRepo.ExistsAsync(whereFn, token);
  }

  /// <summary>
  /// Removes the movie by the given ID ad user id
  /// </summary>
  /// <param name="userId">The user id</param>
  /// <param name="movieId">The movie id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True if deleted, otherwise false</returns>
  public async Task<bool> DeleteByUserAndPkAsync(long userId, long movieId, CancellationToken token = default) {
    return await DeleteAsync(x => x.UserId == userId && x.Id == movieId, token) > 0;
  }

  /*/// <summary>
  /// Fetch the movie by the given owner user id and slug
  /// </summary>
  /// <param name="userId">The user id</param>
  /// <param name="slug">Movie Slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  /// <exception cref="ValidationException">When there is no record found</exception>
  public Task<Movie> GetByUserIdAndSlugAsync(
    long userId, string slug, CancellationToken token = default) {
    return GetOneAsync(x => x.UserId == userId && x.Slug == slug, token);
  }

  /// <summary>
  /// Returns all the movies by owner user id
  /// </summary>
  /// <param name="userId">The user id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched movies</returns>
  public Task<List<Movie>> GetAllByUserAsync(long userId, CancellationToken token = default) {
    return movieRepo.GetManyAsync(x => x.UserId == userId, token);
  }
  
  /// <summary>
  /// Removes the movie by the given ID ad user id
  /// </summary>
  /// <param name="id">The movie id</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True if deleted, otherwise false</returns>
  public async Task<bool> DeleteByPkAsync(long id, CancellationToken token = default) {
    return await DeleteAsync(x => x.Id == id, token) > 0;
  }

  /// <summary>
  /// Verify that the given title exists against year of release or not
  /// </summary>
  /// <param name="title">The movie title</param>
  /// <param name="yearOfRelease">Year of the release</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the movie exists</returns>
  public Task<bool> TitleByYearExistsAsync(
    string title, short yearOfRelease, CancellationToken token = default
  ) {
    return ExistsAsync(x => x.Title == title && x.YearOfRelease == yearOfRelease, token);
  }*/
}