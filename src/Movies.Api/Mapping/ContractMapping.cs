// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public static class ContractMapping {
  public static Movie MapToMovie(this CreateMovieRequest request) {
    return new Movie {
      Title = request.Title,
      YearOfRelease = request.YearOfRelease,
      Genres = request.Genres.ToList(),
    };
  }

  public static MovieResponse MapToResponse(this Movie movie) {
    return new MovieResponse {
      Id = movie.Id,
      Title = movie.Title,
      Slug = movie.Slug,
      YearOfRelease = movie.YearOfRelease,
      Genres = movie.Genres.ToList(),
    };
  }

  public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies) {
    return new() {
      Items = movies.Select(MapToResponse)
    };
  }
  
  public static Movie MapToMovie(this UpdateMovieRequest request, long id) {
    return new Movie {
      Id = id,
      Title = request.Title,
      YearOfRelease = request.YearOfRelease,
      Genres = request.Genres.ToList(),
    };
  }
}