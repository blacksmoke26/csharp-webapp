// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public abstract class DatabaseConfigurator : IServiceConfigurator {
  /// <summary>
  /// Configures the database to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    services.AddDatabase(config);
  }
}