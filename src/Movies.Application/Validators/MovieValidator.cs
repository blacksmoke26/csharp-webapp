// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class MovieValidator : AbstractValidator<Movie> {
  private readonly IMovieRepository _movieRepository;

  public MovieValidator(IMovieRepository movieRepository) {
    _movieRepository = movieRepository;

    /*RuleFor(x => x.Id)
      .MustAsync(ValidateIdAsync);*/

    RuleFor(x => x.Title)
      .NotEmpty();

    RuleFor(x => x.Genres)
      .NotEmpty();

    RuleFor(x => x.YearOfRelease)
      .LessThanOrEqualTo(DateTime.UtcNow.Year);

    RuleFor(x => x.Slug)
      .MustAsync(ValidateSlugAsync)
      .WithMessage("This movie is already exist in our system.");
  }

  /*private async Task<bool> ValidateIdAsync(Movie movie, long? id, CancellationToken token) {
    var existingMovie = await _movieService.GetBySlugAsync(slug);

    if (existingMovie is not null) {
      return existingMovie.Id == movie.Id;
    }

    return existingMovie is null;
  }*/

  private async Task<bool> ValidateSlugAsync(Movie movie, string slug, CancellationToken token) {
    var existingMovie = await _movieRepository.GetBySlugAsync(slug);

    if (existingMovie is not null) {
      return existingMovie.Id == movie.Id;
    }

    return existingMovie is null;
  }
}