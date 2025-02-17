// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator) : IMovieService {
  /// <inheritdoc/>
  public async Task<bool> CreateAsync(Movie movie) {
    await movieValidator.ValidateAndThrowAsync(movie);
    return await movieRepository.CreateAsync(movie);
  }

  /// <inheritdoc/>
  public Task<Movie?> GetByIdAsync(long id) {
    return movieRepository.GetByIdAsync(id);
  }

  /// <inheritdoc/>
  public Task<Movie?> GetBySlugAsync(string slug) {
    return movieRepository.GetBySlugAsync(slug);
  }

  /// <inheritdoc/>
  public Task<IEnumerable<Movie>> GetAllAsync() {
    return movieRepository.GetAllAsync();
  }

  /// <inheritdoc/>
  public async Task<Movie?> UpdateAsync(Movie movie) {
    await movieValidator.ValidateAndThrowAsync(movie);

    if (movie.Id is null) return null;

    var exists = await movieRepository.ExistsByIdAsync((long)movie.Id);
    if (!exists) return null;

    await movieRepository.UpdateAsync(movie);
    return movie;
  }

  /// <inheritdoc/>
  public Task<bool> DeleteByIdAsync(long id) {
    return movieRepository.DeleteByIdAsync(id);
  }
}