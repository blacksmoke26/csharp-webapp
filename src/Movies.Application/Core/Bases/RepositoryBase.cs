// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Application.Core.Interfaces;

namespace Movies.Application.Core.Bases;

public abstract class RepositoryBase<TEntity> :
  IRepositoryDbContext<TEntity>,
  IDbRecordFetch<TEntity> where TEntity : ModelBase {
  /// <inheritdoc/>
  public abstract DatabaseContext GetDbContext();

  /// <inheritdoc/>
  public abstract DbSet<TEntity> GetDataSet();

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default) {
    return GetOneAsync(x => x.Where(whereFn), token);
  }

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken token = default) {
    return GetOneAsync(query, null, token);
  }

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    Expression<Func<TEntity, TEntity>>? selector = null, CancellationToken token = default) =>
    GetOneAsync<TEntity>(query, selector, token);

  /// <inheritdoc/>
  public Task<TResult?> GetOneAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? query,
    Expression<Func<TEntity, TResult>>? selector = null, CancellationToken token = default) {
    var queryable = (query != null ? query.Invoke(GetDataSet()) : GetDataSet()).AsSplitQuery();

    return (
      selector != null
        ? queryable.Select(selector)
        : queryable.Cast<TResult>()
    ).SingleOrDefaultAsync(token);
  }

  /// <inheritdoc/>
  public Task<List<TEntity>> GetManyAsync(
    Expression<Func<TEntity, bool>>? where = null, CancellationToken token = default) {
    return GetManyAsync(x => where != null ? x.Where(where) : x, token);
  }

  /// <inheritdoc/>
  public Task<List<TEntity>> GetManyAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query, CancellationToken token = default) {
    return GetManyAsync<TEntity>(query, null, token);
  }

  /// <inheritdoc/>
  public Task<List<TResult>> GetManyAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? query,
    Expression<Func<TEntity, TResult>>? selector = null,
    CancellationToken token = default) {
    var queryable = (query != null ? query.Invoke(GetDataSet()) : GetDataSet()).AsSplitQuery();

    return (
      selector != null
        ? queryable.Select(selector)
        : queryable.Cast<TResult>()
    ).ToListAsync(token);
  }

  /// <inheritdoc/>
  public Task<bool> ExistsAsync(
    Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default) {
    return GetDataSet().AnyAsync(whereFn, token);
  }

  /// <inheritdoc/>
  public Task<int> DeleteAsync(
    Expression<Func<TEntity, bool>> whereFn, CancellationToken token = default) {
    return GetDataSet()
      .Where(whereFn)
      .ExecuteDeleteAsync(token);
  }
}