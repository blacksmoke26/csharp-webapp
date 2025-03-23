// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.HttpOverrides;
using Movies.Api.Core.Configurators;

namespace Movies.Api.Core.Extensions;

public static class WebApplicationExtensions {
  /// <summary>
  /// Registers web application level services 
  /// </summary>
  /// <param name="app">WebApplication instance</param>
  public static void UseBootstrapper(this WebApplication app) {
    RequestDecompressionConfigurator.Use(app);
    ErrorHandlerConfigurator.Use(app);

    ControllersConfigurator.Use(app);
    
    CorsConfigurator.Use(app);
    
    app.UseForwardedHeaders(new ForwardedHeadersOptions {
      ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    JwtAuthenticationConfigurator.Use(app);
    
    RateLimiterConfigurator.Use(app);
    HealthCheckConfigurator.Use(app);

    //  Warning: Must be called before caching when using the CORS middleware.
    ResponseCachingConfigurator.Use(app);
    OutputCachingConfigurator.Use(app);

    SwaggerConfigurator.Use(app);
    MiddlewareConfigurator.Use(app);
  }
}