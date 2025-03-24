// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Core.Extensions;
using Movies.Application.Core.Interfaces;

namespace Movies.Application.Core.Bases;

/// <summary>
/// This class serves as the base class for all repository classes
/// </summary>
/// <typeparam name="TEntity">The model entity</typeparam>
public abstract class RepositoryBase<TEntity> :
  IRepositoryDbContext<TEntity>,
  IQueryGetOne<TEntity>,
  IQueryGetMany<TEntity>,
  IQueryCountable<TEntity>,
  IQueryExists<TEntity>,
  IQueryDeletable<TEntity>,
  IQueryPaginated<TEntity>,
  IDbSaveChanges<TEntity>
  where TEntity : ModelBase {
  /// <inheritdoc/>
  public abstract MovieDbContext GetDbContext();

  /// <inheritdoc/>
  public abstract DbSet<TEntity> GetDataSet();

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) {
    return GetOneAsync(x => x.Where(predicate), token);
  }

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default) {
    return GetOneAsync(queryable, null, token);
  }

  /// <inheritdoc/>
  public Task<TEntity?> GetOneAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable,
    Expression<Func<TEntity, TEntity>>? selector = null, CancellationToken token = default) =>
    GetOneAsync<TEntity>(queryable, selector, token);

  /// <inheritdoc/>
  public Task<TResult?> GetOneAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TResult>>? selector = null, CancellationToken token = default) {
    var q = (queryable != null
      ? queryable.Invoke(GetDataSet())
      : GetDataSet()).AsSplitQuery();

    return (
      selector != null
        ? q.Select(selector)
        : q.Cast<TResult>()
    ).SingleOrDefaultAsync(token);
  }

  /// <inheritdoc/>
  public Task<List<TEntity>> GetManyAsync(
    Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default) {
    return GetManyAsync(x
      => predicate != null ? x.Where(predicate) : x, token);
  }

  /// <inheritdoc/>
  public Task<List<TEntity>> GetManyAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default) {
    return GetManyAsync<TEntity>(queryable, null, token);
  }

  /// <inheritdoc/>
  public Task<List<TResult>> GetManyAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TResult>>? selector = null,
    CancellationToken token = default) {
    var q = (queryable != null
      ? queryable.Invoke(GetDataSet())
      : GetDataSet()).AsSplitQuery();

    return (
      selector != null
        ? q.Select(selector)
        : q.Cast<TResult>()
    ).ToListAsync(token);
  }

  /// <inheritdoc/>
  public Task<int> DeleteAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) {
    return GetDataSet().Where(predicate).ExecuteDeleteAsync(token);
  }

  /// <inheritdoc/>
  public Task<int> DeleteAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default) {
    return GetDataSet().AddQuery(queryable).ExecuteDeleteAsync(token);
  }

  /// <inheritdoc/>
  public Task<bool> ExistsAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) {
    return GetDataSet().AnyAsync(predicate, token);
  }

  /// <inheritdoc/>
  public Task<bool> ExistsAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default) {
    return GetDataSet().AddQuery(queryable).AnyAsync(token);
  }

  /// <inheritdoc/>
  public Task<int> CountAsync(
    Expression<Func<TEntity, bool>> predicate, CancellationToken token = default) {
    return GetDataSet().CountAsync(predicate, token);
  }

  /// <inheritdoc/>
  public Task<int> CountAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> queryable, CancellationToken token = default) {
    return GetDataSet().AddQuery(queryable).CountAsync(token);
  }

  /// <inheritdoc/>
  public Task<PaginatedList<TEntity>> GetPaginatedAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TEntity>>? selector, PaginatorOptions options,
    CancellationToken token = default) {
    return GetPaginatedAsync<TEntity>(queryable, selector, options, token);
  }

  /// <inheritdoc/>
  public Task<PaginatedList<TResult>> GetPaginatedAsync<TResult>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryable,
    Expression<Func<TEntity, TResult>>? selector, PaginatorOptions options,
    CancellationToken token = default) {
    var source = (queryable != null
      ? queryable.Invoke(GetDataSet())
      : GetDataSet()).AsSplitQuery();

    return PaginatedList<TEntity>.CreateWithSelectorAsync(source, selector, options, token);
  }

  /// <inheritdoc/>
  public async Task<TEntity?> SaveAsync(TEntity entity, CancellationToken token = default) {
    GetDataSet().Update(entity).State = EntityState.Modified;
    return await GetDbContext().SaveChangesAsync(token) > 0 ? entity : null;
  }
}