// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Core.Interfaces;

public interface IServiceRepoInstance<TModel> {
  /// <summary>
  /// Returns the `Repository` instance constructed by this service
  /// </summary>
  /// <returns>The Repository instance</returns>
  public TModel GetRepo();
}