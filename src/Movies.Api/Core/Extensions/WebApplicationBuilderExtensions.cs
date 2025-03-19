// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.HttpOverrides;
using Movies.Api.Core.Middleware;
using Movies.Api.Core.Swagger;

namespace Movies.Api.Core.Extensions;

public static class BootstrapperWebApplicationBuilderExtensions {
  /// <summary>
  /// Registers web application level services 
  /// </summary>
  /// <param name="app">WebApplication instance</param>
  /// <returns>The updated app instance</returns>
  public static WebApplication UseBootstrapper(this WebApplication app) {
    #region JWT Authentication / Authorization

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseForwardedHeaders(new ForwardedHeadersOptions {
      ForwardedHeaders
        = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    #endregion
    
    app.UseSwaggerApi();
    
    app.UseExceptionHandler();
    app.UseStatusCodePages();
    
    // Map controllers
    app.MapControllers();

    app.MapMiddleware();
    
    return app;
  }
}