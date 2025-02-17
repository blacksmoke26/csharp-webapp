// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;

namespace Movies.Application.Services;

public interface IMovieService {
  Task<bool> CreateAsync(Movie movie);

  Task<Movie?> GetByIdAsync(long id);
  
  Task<Movie?> GetBySlugAsync(string slug);

  Task<IEnumerable<Movie>> GetAllAsync();

  Task<Movie?> UpdateAsync(Movie movie);

  Task<bool> DeleteByIdAsync(long id);
}