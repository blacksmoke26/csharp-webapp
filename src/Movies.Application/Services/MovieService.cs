// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator) : IMovieService {
  /// <inheritdoc/>
  public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default) {
    await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
    return await movieRepository.CreateAsync(movie, token);
  }

  /// <inheritdoc/>
  public Task<Movie?> GetByIdAsync(long id, CancellationToken token = default) {
    return movieRepository.GetByIdAsync(id, token);
  }

  /// <inheritdoc/>
  public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default) {
    return movieRepository.GetBySlugAsync(slug, token);
  }

  /// <inheritdoc/>
  public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default) {
    return movieRepository.GetAllAsync(token);
  }

  /// <inheritdoc/>
  public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default) {
    await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);

    if (movie.Id is null) return null;

    var exists = await movieRepository.ExistsByIdAsync((long)movie.Id, token);
    if (!exists) return null;

    await movieRepository.UpdateAsync(movie, token);
    return movie;
  }

  /// <inheritdoc/>
  public Task<bool> DeleteByIdAsync(long id, CancellationToken token = default) {
    return movieRepository.DeleteByIdAsync(id, token);
  }
}