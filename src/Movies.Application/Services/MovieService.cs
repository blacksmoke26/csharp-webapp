// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository) : IMovieService {
  public Task<bool> CreateAsync(Movie movie) {
    return movieRepository.CreateAsync(movie);
  }

  public Task<Movie?> GetByIdAsync(long id) {
    return movieRepository.GetByIdAsync(id);
  }

  public Task<Movie?> GetBySlugAsync(string slug) {
    return movieRepository.GetBySlugAsync(slug);
  }

  public Task<IEnumerable<Movie>> GetAllAsync() {
    return movieRepository.GetAllAsync();
  }

  public async Task<Movie?> UpdateAsync(Movie movie) {
    if (movie.Id is null) return null;

    var exists = await movieRepository.ExistsByIdAsync((long)movie.Id);
    if (!exists) return null;

    await movieRepository.UpdateAsync(movie);
    return movie;
  }

  public Task<bool> DeleteByIdAsync(long id) {
    return movieRepository.DeleteByIdAsync(id);
  }
}