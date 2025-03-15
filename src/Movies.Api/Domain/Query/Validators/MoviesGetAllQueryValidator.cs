// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api.Domain.Query.Validators;

public class MoviesGetAllQueryValidator: AbstractValidator<MoviesGetAllQuery> {
  public MoviesGetAllQueryValidator() {
    RuleFor(x => x.Title)
      .MaximumLength(50);

    RuleFor(x => x.Year)
      .InclusiveBetween((short)1900, (short)DateTime.UtcNow.Year);
  }
}