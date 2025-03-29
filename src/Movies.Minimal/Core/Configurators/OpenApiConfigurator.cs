// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata
// Versioning: https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format
// See also: https://dev.to/bytehide/how-to-integrate-openapi-into-your-aspnet-core-projects-with-net-9-399a
// See also: https://rssfeedtelegrambot.bnaya.co.il/index.php/2024/11/25/openapi-document-generation-in-net-9/

using Microsoft.OpenApi.Models;
using Movies.Api.Core.Interfaces;
using Movies.Application.Config;
using Movies.Minimal.Core.OpenApi;

namespace Movies.Minimal.Core.Configurators;

/// <summary>
/// This configurator class is used to configure the <see href="https://aka.ms/aspnet/openapi">OpenAPI</see>
/// specification generator
/// </summary>
public abstract class OpenApiConfigurator : IApplicationServiceConfigurator {
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    foreach (var version in ApiVersions.VersionsList) {
      var formattedVersion = version.ToString(ApiVersions.VersionFormat);

      // Add services to the container.
      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      services.AddOpenApi(formattedVersion, options => {
        options.AddDocumentTransformer((document, _, _) => {
          document.Info = new() {
            Title = $"Movies API | {version}",
            Version = formattedVersion,
            Contact = new OpenApiContact {
              Name = "Junaid Atari",
              Email = "mj.atari@gmail.com",
              Url = new Uri("https://github.com/blacksmoke26")
            },
            License = new OpenApiLicense {
              Name = "Apache 2.0",
              Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
          };

          return Task.CompletedTask;
        });

        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        options.AddDocumentTransformer<VersionParameterTransformer>();
        options.AddOperationTransformer<DeprecatedOperationTransformer>();
        options.AddOperationTransformer<ParametersCamelcaseOperationTransformer>();
        options.AddSchemaTransformer<SchemaTransformer>();
      });
    }
  }

  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.MapOpenApi();
  }
}