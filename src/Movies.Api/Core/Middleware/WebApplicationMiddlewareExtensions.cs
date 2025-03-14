// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api.Core.Middleware;

public static class WebApplicationMiddlewareExtensions {
  public static WebApplication MapMiddleware(this WebApplication app) {
    app.UseMiddleware<ValidationMappingMiddleware>();
    app.UseMiddleware<AuthValidationMiddleware>();
    
    return app;
  }
}