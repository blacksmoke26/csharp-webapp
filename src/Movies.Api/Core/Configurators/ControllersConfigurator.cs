// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Movies.Api.Core.Configurators;

public abstract class ControllersConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the MVC controllers to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    // Add services to the container.
    services.AddControllers()
      .AddNewtonsoftJson(options => {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.Add(new StringEnumConverter
          { NamingStrategy = new SnakeCaseNamingStrategy() });
      });
  }

  /// <summary>
  /// Configures the MVC controllers to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.MapControllers();
  }
}