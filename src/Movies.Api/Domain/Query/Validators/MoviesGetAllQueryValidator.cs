// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Requests.Validators;

namespace Movies.Api.Domain.Query.Validators;

public class MoviesGetAllQueryValidator : AbstractValidator<MoviesGetAllQuery> {
  public MoviesGetAllQueryValidator() {
    RuleFor(x => x.Title)
      .MaximumLength(50);

    RuleFor(x => x.UserId)
      .GreaterThan(0);

    RuleFor(x => x.Year)
      .InclusiveBetween(MovieFilters.YearMin, MovieFilters.YearMax);

    Include(new QueryFetchingValidator<MoviesGetAllQuery> {
      SortByFields = MovieFilters.SortByFields
    });
  }
}