// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public abstract class OutputCachingConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the output caching to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    services.AddOutputCache(x => {
      x.AddBasePolicy(c => c.Cache());
      InitializePolicies(x);
    });
  }

  /// <summary>
  /// Configures the output caching to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    // Note:
    // - Only status with 200 are cached 
    // - Only GET and HEAD requests are cached 
    // - Set cookies are not cached 
    // - Authentication requests are not cached 
    app.UseOutputCache();
  }
  
  /// <summary>
  /// Builds the output caching policies
  /// </summary>
  /// <param name="opt">The <see cref="Microsoft.AspNetCore.OutputCaching.OutputCacheOptions">OutputCacheOptions</see> options</param>
  private static void InitializePolicies(OutputCacheOptions opt) {
    // Add `Movies` policy
    opt.AddPolicy(CachePolicies.Movies.Policy, c => {
      c.Cache()
        .Expire(TimeSpan.FromMinutes(1))
        .SetVaryByQuery(CachePolicies.Movies.VaryByQuery)
        .Tag(CachePolicies.Movies.Policy);
    });
  }
}