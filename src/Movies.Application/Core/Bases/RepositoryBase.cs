// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Core.Interfaces;
using Movies.Application.Database;

namespace Movies.Application.Core.Bases;

public abstract class RepositoryBase<TModel> :
  IRepositoryDbContext<TModel>,
  IDbRecordFetch<TModel> where TModel : ModelBase {
  /// <inheritdoc/>
  public abstract DatabaseContext GetDbContext();

  /// <inheritdoc/>
  public abstract DbSet<TModel> GetDataSet();

  /// <inheritdoc/>
  public Task<TModel?> GetOneAsync(Expression<Func<TModel, bool>> whereFn, CancellationToken token = default) {
    return GetDataSet()
      .SingleOrDefaultAsync(whereFn, cancellationToken: token);
  }

  /// <inheritdoc/>
  public Task<List<TModel>> GetManyAsync(
    Expression<Func<TModel, bool>>? whereFn = null, CancellationToken token = default) {
    return whereFn is null
      ? GetDataSet().ToListAsync(token)
      : GetDataSet()
        .Where(whereFn)
        .ToListAsync(token);
  }

  /// <inheritdoc/>
  public Task<bool> ExistsAsync(Expression<Func<TModel, bool>> whereFn, CancellationToken token = default) {
    return GetDataSet().AnyAsync(whereFn, token);
  }

  /// <inheritdoc/>
  public Task<int> DeleteAsync(Expression<Func<TModel, bool>> whereFn, CancellationToken token = default) {
    return GetDataSet()
      .Where(whereFn)
      .ExecuteDeleteAsync(token);
  }
}