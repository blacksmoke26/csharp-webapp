// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/request-decompression
// See more: https://dotnetfullstackdev.medium.com/request-decompression-middleware-f3e061d93b55
// See more: https://www.infoworld.com/article/2338382/how-to-use-request-decompression-in-aspnet-core-7.html

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public abstract class RequestDecompressionConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the request decompression to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    services.AddRequestDecompression();
  }

  /// <summary>
  /// Configures the request decompression to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.UseRequestDecompression();
  }
}