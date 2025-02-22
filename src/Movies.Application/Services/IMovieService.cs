// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IMovieService {
  /// <summary>
  /// Creates a movie
  /// </summary>
  /// <param name="movie">The movie object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Whatever the movie is created or not</returns>
  Task<bool> CreateAsync(Movie movie, CancellationToken token = default);

  /// <summary>
  /// Fetch the movie by the given ID
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  Task<Movie?> GetByIdAsync(long id, CancellationToken token = default);

  /// <summary>
  /// Fetch the movie by the given slug
  /// </summary>
  /// <param name="slug">Movie Slug</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default);

  /// <summary>
  /// Returns all the movies
  /// </summary>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched movies</returns>
  Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default);

  /// <summary>
  /// Updates a movie
  /// </summary>
  /// <param name="movie">The movie object</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The updated object, otherwise null when failed</returns>
  Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default);

  /// <summary>
  /// Removes the movie by the given ID
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True if deleted, otherwise false</returns>
  Task<bool> DeleteByIdAsync(long id, CancellationToken token = default);
}