// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository {
  /// <summary>
  /// Creates a movie
  /// </summary>
  /// <param name="movie">The movie object</param>
  /// <returns>Whatever the movie is created or not</returns>
  Task<bool> CreateAsync(Movie movie);

  /// <summary>
  /// Fetch the movie by the given ID
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  Task<Movie?> GetByIdAsync(long id);

  /// <summary>
  /// Fetch the movie by the given slug
  /// </summary>
  /// <param name="slug">Movie Slug</param>
  /// <returns>The fetched object, otherwise null if not found</returns>
  Task<Movie?> GetBySlugAsync(string slug);

  /// <summary>
  /// Returns all the movies
  /// </summary>
  /// <returns>The fetched movies</returns>
  Task<IEnumerable<Movie>> GetAllAsync();

  /// <summary>
  /// Updates a movie
  /// </summary>
  /// <param name="movie">The movie object</param>
  /// <returns>Whatever the movie is updated or not</returns>
  Task<bool> UpdateAsync(Movie movie);

  /// <summary>
  /// Removes the movie by the given ID
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <returns>True if deleted, otherwise false</returns>
  Task<bool> DeleteByIdAsync(long id);
  
  /// <summary>
  /// Validates the given movie id exists
  /// </summary>
  /// <param name="id">Movie ID</param>
  /// <returns>Whatever the movie is exists</returns>
  Task<bool> ExistsByIdAsync(long id);
}