// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://github.com/dotnet/aspnet-api-versioning/wiki/API-Documentation#aspnet-core
// Read more: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle

using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Core.Swagger;

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

      // using System.Reflection;
      var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
  }
}