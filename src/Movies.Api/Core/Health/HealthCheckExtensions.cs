// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks#database-probe

namespace Movies.Api.Core.Health;

public static class HealthCheckExtensions {
  public static IServiceCollection AddHealthCheck(
    this IServiceCollection services
  ) {
    services.AddHealthChecks()
      .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);

    return services;
  }

  public static WebApplication UseHealthCheck(this WebApplication app) {
    app.MapHealthChecks("health-check");
    return app;
  }
}