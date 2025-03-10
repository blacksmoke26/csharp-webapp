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
  public Task<TModel?> GetOneAsync(
    Expression<Func<TModel, bool>> whereFn, CancellationToken token = default) {
    return GetOneAsync(x => x.Where(whereFn), token);
  }

  /// <inheritdoc/>
  public Task<TModel?> GetOneAsync(
    Func<IQueryable<TModel>, IQueryable<TModel>> queryableFn, CancellationToken token = default) {
    return GetOneAsync<TModel>(queryableFn, token);
  }

  /// <inheritdoc/>
  public Task<TReturn?> GetOneAsync<TReturn>(
    Func<IQueryable<TModel>, IQueryable<TModel>> queryableFn, CancellationToken token = default) {
    return queryableFn.Invoke(GetDataSet().AsQueryable()).Cast<TReturn>().SingleOrDefaultAsync(token);
  }

  /// <inheritdoc/>
  public Task<List<TModel>> GetManyAsync(
    Func<IQueryable<TModel>, IQueryable<TModel>> queryableFn,
    CancellationToken token = default) {
    return GetManyAsync<TModel>(queryableFn, token);
  }

  /// <inheritdoc/>
  public Task<List<TModel>> GetManyAsync(Expression<Func<TModel, bool>>? whereFn = null,
    CancellationToken token = default) {
    return GetManyAsync<TModel>(whereFn, token);
  }

  /// <inheritdoc/>
  public Task<List<TReturn>> GetManyAsync<TReturn>(Expression<Func<TModel, bool>>? whereFn = null,
    CancellationToken token = default) {
    return whereFn != null
      ? GetManyAsync<TReturn>(x => x.Where(whereFn), token)
      : GetManyAsync<TReturn>(x => x.Where(_ => true), token);
  }

  /// <inheritdoc/>
  public Task<List<TReturn>> GetManyAsync<TReturn>(Func<IQueryable<TModel>, IQueryable<TModel>> queryableFn,
    CancellationToken token = default) {
    return queryableFn.Invoke(GetDataSet().AsQueryable()).Cast<TReturn>().ToListAsync(token);
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