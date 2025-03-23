// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Requests.Validators;

namespace Movies.Api.Domain.Query.Validators;

public class RatingsGetAllQueryValidator : AbstractValidator<RatingsGetAllQuery> {
  public RatingsGetAllQueryValidator() {
    RuleFor(x => x.Score).GreaterThan((short)0);

    RuleFor(x => x.MovieId).GreaterThan(0);

    Include(new QueryFetchingValidator<RatingsGetAllQuery> {
      SortByFields = RatingFilters.SortByFields
    });
  }
}