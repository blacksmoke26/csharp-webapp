// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/security/cors
// See more: https://medium.com/@anton.martyniuk/cross-origin-resource-sharing-cors-in-asp-net-core-a-comprehensive-guide-9542706aedb2
// Middleware order: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware#middleware-order

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public record CorsConfiguration {
  /// <summary>
  ///  Origins to match the normalization performed by the browser on the value sent in the ORIGIN header.
  /// </summary>
  public IEnumerable<string> Origins { get; set; } = [];

  /// <summary>
  /// The headers which need to be allowed in the request.
  /// </summary>
  public IEnumerable<string> Headers { get; set; } = [];

  /// <summary>
  /// The headers which need to be exposed to the client.
  /// </summary>
  public IEnumerable<string> ExposedHeaders { get; set; } = [];
}

public abstract class CorsConfigurator : IApplicationServiceConfigurator {
  private static readonly CorsConfiguration? Configuration = new ();
  
  /// <summary>
  /// Configures the CORS to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    config.GetConfig().GetSection("Cors").Bind(Configuration);

    services.AddCors(opt => {
      opt.AddPolicy(CorsPolicies.Default, policy => {
        policy
          .AllowCredentials()
          .WithOrigins(Configuration?.Origins.ToArray() ?? [])
          .WithHeaders(Configuration?.Headers.ToArray() ?? [])
          .WithExposedHeaders(Configuration?.ExposedHeaders.ToArray() ?? [])
          .SetIsOriginAllowed(IsOriginAllowed)
          .SetIsOriginAllowedToAllowWildcardSubdomains();
      });
    });
  }

  /// <summary>
  /// Verifies that the given origin is allowed to pass CORS or not
  /// </summary>
  /// <param name="uri">The host / origin</param>
  /// <returns>True when allowed, false otherwise</returns>
  private static bool IsOriginAllowed(string uri) {
    // Checks the uri against some logic or false to skip this part. 
    return false;
  }

  /// <summary>
  /// Configures the CORS to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.UseCors(CorsPolicies.Default);
  }
}