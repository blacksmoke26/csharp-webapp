// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Core.Configurators;
using Movies.Application.Context;
using Movies.Minimal.Core.Configurators;

namespace Movies.Minimal.Core.Extensions;

public static class WebApplicationExtensions {
  /// <summary>
  /// Registers web application level services 
  /// </summary>
  /// <param name="app">WebApplication instance</param>
  public static void UseBootstrapper(this WebApplication app) {
    //RequestDecompressionConfigurator.Use(app);
    ErrorHandlerConfigurator.Use(app);

    OpenApiConfigurator.Use(app);
    ScalarClientConfigurator.Use(app);

    //CorsConfigurator.Use(app);

    app.UseForwardedHeaders(new ForwardedHeadersOptions {
      ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    JwtAuthenticationConfigurator.Use(app);

    //RateLimiterConfigurator.Use(app);
    //HealthCheckConfigurator.Use(app);

    //  Warning: Must be called before caching when using the CORS middleware.
    //ResponseCachingConfigurator.Use(app);
    //OutputCachingConfigurator.Use(app);

    //SwaggerConfigurator.Use(app);
    MiddlewareConfigurator.Use(app);
  }

  /// <summary>
  /// Initialize the application 
  /// </summary>
  /// <param name="app">WebApplication instance</param>
  public static async Task InitializeAsync(this WebApplication app) {
    var dbContext = app.Services.GetRequiredService<MovieDbContext>();

    try {
      await dbContext.Database.MigrateAsync();
      await dbContext.Database.EnsureCreatedAsync();
    }
    catch (Exception) {
      // ignored
    }
  }
}