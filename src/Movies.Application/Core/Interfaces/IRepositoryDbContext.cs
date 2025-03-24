// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Core.Interfaces;

public interface IRepositoryDbContext<TModel> where TModel : class {
  /// <summary>
  /// Returns the instance of database context
  /// </summary>
  /// <returns>The database context</returns>
  public MovieDbContext GetDbContext();
  
  /// <summary>
  /// Returns the model specific data-set context instance
  /// </summary>
  /// <returns>The dataset context</returns>
  public DbSet<TModel> GetDataSet();
}