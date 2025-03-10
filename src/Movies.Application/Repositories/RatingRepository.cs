// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.EntityFrameworkCore;
using Movies.Application.Core.Bases;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class RatingRepository(
  DatabaseContext dbContext,
  UserRepository userRepo,
  MovieRepository movieRepo
) : RepositoryBase<Rating> {
  /// <inheritdoc/>
  public override DatabaseContext GetDbContext() => dbContext;

  /// <inheritdoc/>
  public override DbSet<Rating> GetDataSet() => dbContext.Ratings;
}