// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;

namespace Movies.Application.Repositories;

public interface IMovieRepository {
  Task<bool> CreateAsync(Movie movie);

  Task<Movie?> GetByIdAsync(Guid id);

  Task<IEnumerable<Movie>> GetAllAsync(Movie movie);

  Task<bool> UpdateAsync(Movie movie);

  Task<bool> DeleteByIdAsync(Guid id);
}