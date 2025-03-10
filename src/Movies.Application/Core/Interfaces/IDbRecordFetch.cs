// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

public interface IDbRecordFetch<TModel> {
  /// <summary>
  /// Fetches a single record against the given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The found record, null if there is not</returns>
  public Task<TModel?> GetOneAsync(Func<TModel, bool> whereFn, CancellationToken token = default);

  /// <summary>
  /// Fetches the multiple records against the given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>The fetched records</returns>
  public Task<List<TModel>> GetManyAsync(Func<TModel, bool>? whereFn = null, CancellationToken token = default);
  
  /// <summary>
  /// Checks the records exists against the given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>True upon exist, false otherwise</returns>
  public Task<bool> ExistsAsync(Func<TModel, bool> whereFn, CancellationToken token = default);
  
  /// <summary>
  /// Deletes one or many records against then given condition
  /// </summary>
  /// <param name="whereFn">A callback which apply as a where condition</param>
  /// <param name="token">The cancellation token</param>
  /// <returns>Returns the count of deleted records</returns>
  public Task<int> DeleteAsync(Func<TModel, bool> whereFn, CancellationToken token = default);
}