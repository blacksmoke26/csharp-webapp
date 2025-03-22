// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See more: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview
// Guide: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output

namespace Movies.Api.Core.Caching;

public static class OutputCacheApplicationExtensions {
  /// <summary>
  /// Add output caching services and configure the related options.
  /// </summary>
  /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for adding services.</param>
  /// <returns></returns>
  public static IServiceCollection AddOutputCaching(this IServiceCollection services) {
    services.AddOutputCache(x => {
      x.AddBasePolicy(c => c.Cache());
      OutputCachePoliciesBuilder.Build(x);
    });

    return services;
  }

  /// <summary>
  /// Adds the <see cref="T:Microsoft.AspNetCore.OutputCaching.OutputCacheMiddleware" /> for caching HTTP responses.
  /// </summary>
  /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
  public static WebApplication UseOutputCaching(this WebApplication app) {
    // Note:
    // - Only status with 200 are cached 
    // - Only GET and HEAD requests are cached 
    // - Set cookies are not cached 
    // - Authentication requests are not cached 
    app.UseOutputCache();

    return app;
  }
}