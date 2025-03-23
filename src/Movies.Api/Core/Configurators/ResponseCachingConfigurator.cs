// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/response
// Middleware: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/middleware
// See more: https://dotnettutorials.net/lesson/response-caching-in-asp-net-core/
// See more: https://medium.com/net-newsletter-by-waseem/episode-18-what-is-response-caching-and-how-to-implement-it-in-net-core-402aa2a20688
// See more: https://code-maze.com/aspnetcore-response-caching/

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public abstract class ResponseCachingConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the response caching to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
  }

  /// <summary>
  /// Configures the response caching to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
  }
}