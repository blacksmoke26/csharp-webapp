// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository {
  private readonly List<Movie> _movies = [];

  public Task<bool> CreateAsync(Movie movie) {
    _movies.Add(movie);
    return Task.FromResult(true);
  }

  public Task<Movie?> GetByIdAsync(Guid id) {
    var movie = _movies.SingleOrDefault(m => m.Id == id);
    return Task.FromResult(movie);
  }

  public Task<Movie?> GetBySlugAsync(string slug) {
    var movie = _movies.SingleOrDefault(m => m.Slug == slug);
    return Task.FromResult(movie);
  }

  public Task<IEnumerable<Movie>> GetAllAsync() {
    return Task.FromResult(_movies.AsEnumerable());
  }

  public Task<bool> UpdateAsync(Movie movie) {
    var index = _movies.FindIndex(m => m.Id == movie.Id);

    if (index == -1)
      return Task.FromResult(false);

    _movies[index] = movie;
    return Task.FromResult(true);
  }

  public Task<bool> DeleteByIdAsync(Guid id) {
    var count = _movies.RemoveAll(m => m.Id == id);
    return Task.FromResult(count > 0);
  }
}