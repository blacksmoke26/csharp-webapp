// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository {
  private readonly List<Movie> _movies = [];

  public Task<bool> CreateAsync(Movie movie) {
    throw new NotImplementedException();
  }

  public Task<Movie?> GetByIdAsync(Guid id) {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<Movie>> GetAllAsync(Movie movie) {
    throw new NotImplementedException();
  }

  public Task<bool> UpdateAsync(Movie movie) {
    throw new NotImplementedException();
  }

  public Task<bool> DeleteByIdAsync(Guid id) {
    throw new NotImplementedException();
  }
}