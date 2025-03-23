// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Reference: https://scalar.com/#introduction
// Integration guide: https://guides.scalar.com/scalar/scalar-api-references/net-integration
// See: https://juldhais.net/create-a-beautiful-api-documentation-with-scalar-in-asp-net-core-d3d4d17570a6

using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Movies.Api.Core.Filters.Swagger;
using Movies.Api.Core.Interfaces;
using Movies.Application.Config;
using Scalar.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Core.Configurators;

public abstract class SwaggerConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the swagger / scalar to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    services.AddSwaggerGen(x => {
      x.OperationFilter<VersioningOperationFilter>();
      x.OperationFilter<ObsoleteOperationFilter>();
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

  /// <summary>
  /// Configures the swagger / scalar to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
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

public class ConfigureSwaggerOptions(
  IApiVersionDescriptionProvider provider,
  IHostEnvironment environment)
  : IConfigureOptions<SwaggerGenOptions> {
  /// <inheritdoc/>
  public void Configure(SwaggerGenOptions options) {
    options.EnableAnnotations();

    foreach (var description in provider.ApiVersionDescriptions) {
      options.SwaggerDoc(
        description.GroupName,
        new OpenApiInfo {
          Title = $"Movies API ({environment.EnvironmentName})",
          Description = description.ApiVersion.ToString(),
          Version = description.ApiVersion.ToString(),
          Contact = new OpenApiContact {
            Name = "Junaid Atari",
            Email = "mj.atari@gmail.com",
            Url = new Uri("https://github.com/blacksmoke26")
          },
          License = new OpenApiLicense {
            Name = "Apache 2.0",
            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
          }
        }
      );
    }

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
  }
}