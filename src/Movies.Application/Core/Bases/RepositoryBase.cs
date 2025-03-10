// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

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
  public Task<TModel?> GetOneAsync(Func<TModel, bool> whereFn, CancellationToken token = default) {
    return GetDataSet()
      .SingleOrDefaultAsync(x => whereFn.Invoke(x), cancellationToken: token);
  }

  /// <inheritdoc/>
  public Task<List<TModel>> GetManyAsync(Func<TModel, bool>? whereFn = null, CancellationToken token = default) {
    return GetDataSet()
      .Where(x => whereFn == null || whereFn.Invoke(x))
      .ToListAsync(token);
  }

  /// <inheritdoc/>
  public Task<bool> ExistsAsync(Func<TModel, bool> whereFn, CancellationToken token = default) {
    return GetDataSet().AnyAsync(x => whereFn.Invoke(x), token);
  }

  /// <inheritdoc/>
  public Task<int> DeleteAsync(Func<TModel, bool> whereFn, CancellationToken token = default) {
    return GetDataSet()
      .Where(x => whereFn.Invoke(x))
      .ExecuteDeleteAsync(token);
  }
}