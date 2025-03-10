// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Core.Bases;
using Movies.Application.Core.Interfaces;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class RatingService(RatingRepository ratingRepo)
  : ServiceBase, IServiceRepoInstance<RatingRepository> {
  /// <inheritdoc/>
  public RatingRepository GetRepo() => ratingRepo;
}