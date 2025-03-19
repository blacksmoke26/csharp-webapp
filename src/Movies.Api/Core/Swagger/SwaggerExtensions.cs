// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Core.Swagger;

/// <summary>
/// This class represents the extension methods for enabling the Swagger through the application
/// </summary>
public static class SwaggerExtensions {
  /// <summary>Register the swagger core and UI</summary>
  /// <param name="services">The ServiceCollection instance</param>
  public static void AddSwagger(this IServiceCollection services) {
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    services.AddSwaggerGen(x => {
      x.OperationFilter<SwaggerDefaultValues>();
      x.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
      });
      x.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
          new OpenApiSecurityScheme {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
          },
          []
        }
      });
    });

    services.AddSwaggerGenNewtonsoftSupport();
  }

  /// <summary>Enables the swagger middleware and UI</summary>
  /// <param name="app">The WebApplication instance</param>
  public static void UseSwaggerApi(this WebApplication app) {
    app.UseSwagger();
    app.UseSwaggerUI(x => {
      foreach (var description in app.DescribeApiVersions()) {
        x.SwaggerEndpoint(
          $"/swagger/{description.GroupName}/swagger.json",
          description.GroupName.ToUpperInvariant()
        );
      }
    });
  }
}