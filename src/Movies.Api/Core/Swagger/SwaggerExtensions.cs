// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Reference: https://scalar.com/#introduction
// Integration guide: https://guides.scalar.com/scalar/scalar-api-references/net-integration
// See: https://juldhais.net/create-a-beautiful-api-documentation-with-scalar-in-asp-net-core-d3d4d17570a6

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
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
    var versions = app.DescribeApiVersions()
      .Select(x => x.GroupName).ToArray();
    app.UseSwagger(opt => { opt.RouteTemplate = "openapi/{documentName}.json"; });
    
    app.MapScalarApiReference(opt => {
      opt.AddDocuments(versions);
      opt.Title = $"Movies: API Reference ({app.Environment.EnvironmentName})";
      opt.Theme = ScalarTheme.Purple;
      opt.HideClientButton = true;
      opt.HideDownloadButton = true;
    });
  }
}