// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Repositories;

public class GenreRepository(DatabaseContext dbContext) : RepositoryBase<Genre> {
  /// <inheritdoc/>
  public override DatabaseContext GetDbContext() => dbContext;

  /// <inheritdoc/>
  public override DbSet<Genre> GetDataSet() => dbContext.Genres;
}