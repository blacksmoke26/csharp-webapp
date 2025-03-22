// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-9.0

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Movies.Application.Database;

namespace Movies.Api.Core.Health;

public class DatabaseHealthCheck(
  DatabaseContext dbContext,
  ILogger<DatabaseHealthCheck> logger
) : IHealthCheck {
  public const string Name = "Database";
  public const string Message = "Database is unhealthy";

  public async Task<HealthCheckResult> CheckHealthAsync(
    HealthCheckContext context, CancellationToken token = new()) {
    try {
      _ = await dbContext.Database.ExecuteSqlAsync($"SELECT 1", token);
      return HealthCheckResult.Healthy();
    }
    catch (Exception e) {
      logger.LogError(e, Message);
      return HealthCheckResult.Unhealthy(Message, e);
    }
  }
}