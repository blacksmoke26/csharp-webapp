// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Repositories;

public enum MovieStatus {
  Deleted = 0,
  Pending = 1,
  Blocked = 2,
  Draft = 3,
  Published = 10
}

public class MovieRepository(
  MovieDbContext dbContext
) : RepositoryBase<Movie> {
  /// <inheritdoc/>
  public override MovieDbContext GetDbContext() => dbContext;

  /// <inheritdoc/>
  public override DbSet<Movie> GetDataSet() => dbContext.Movies;
}