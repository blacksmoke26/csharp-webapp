// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api.Domain.Query.Validators;

public class MoviesGetAllQueryValidator : AbstractValidator<MoviesGetAllQuery> {
  public MoviesGetAllQueryValidator() {
    RuleFor(x => x.Title)
      .MaximumLength(50);

    RuleFor(x => x.UserId)
      .GreaterThan(0);

    RuleFor(x => x.Year)
      .InclusiveBetween(MovieFilters.YearMin, MovieFilters.YearMax);

    RuleFor(x => x.SortBy)
      .MinimumLength(2)
      .MaximumLength(20)
      .Must(value => value?.Length != 0 || StringHelper.IsSortOrderValue(value))
      .WithMessage("The sort field should start with '-', '+' or a letter character (camel case or snake case only)")
      /*.Must(value => StringHelper.HasSortOrderField(
        value, [MovieFilters.SortByTitle, MovieFilters.SortByYear]))
      .WithMessage("Unknown sort order field name given")*/;
  }
}