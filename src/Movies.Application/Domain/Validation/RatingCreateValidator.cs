// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types

namespace Movies.Application.Domain.Validation;

public class RatingCreateValidator : AbstractValidator<RatingCreateModel> {
  public RatingCreateValidator() {
    RuleFor(x => x.UserId)
      .GreaterThan(0)
      .NotEmpty();

    RuleFor(x => x.MovieId)
      .GreaterThan(0)
      .NotEmpty();

    RuleFor(x => x.Score)
      .InclusiveBetween((short)0, (short)5f)
      .NotNull();

    RuleFor(x => x.Feedback)
      .MaximumLength(1000);
  }
}