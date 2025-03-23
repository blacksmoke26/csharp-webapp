// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Api.Core.Configurators;
using Movies.Application.Config;

namespace Movies.Api.Core.Extensions;

/// <summary>
/// Represents as the part of dependency injection to register the dependencies inside the application
/// </summary>
public static class ServiceCollectionExtensions {
  /// <summary>
  /// Registers application level services 
  /// </summary>
  /// <param name="services">ServiceCollection instance</param>
  /// <param name="configuration">Application configuration</param>
  public static void InitBootstrapper(
    this IServiceCollection services, IConfiguration configuration
  ) {
    var config = new AppConfiguration(configuration);

    services.AddSingleton<AppConfiguration>(_ => config);

    ApiVersioningConfigurator.Configure(services, config);

    OutputCachingConfigurator.Configure(services, config);
    ResponseCachingConfigurator.Configure(services, config);
    
    JwtAuthenticationConfigurator.Configure(services, config);
    ErrorHandlerConfigurator.Configure(services, config);

    ControllersConfigurator.Configure(services, config);
    SwaggerConfigurator.Configure(services, config);

    HealthCheckConfigurator.Configure(services, config);
    RateLimiterConfigurator.Configure(services, config);

    services.AddApplication();
    
    DatabaseConfigurator.Configure(services, config);

    MiddlewareConfigurator.Configure(services, config);
  }
}