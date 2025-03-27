// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://github.com/dotnet/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
// Example: https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/BasicExample
// Wiki: https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Selector

using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

/// <summary>
/// Provides the extensions methods to support API Versioning
/// <p>Read more about
/// <see href="https://github.com/dotnet/aspnet-api-versioning/wiki/API-Versioning-Options">API Versioning Options</see></p>
/// </summary>
public abstract class ApiVersioningConfigurator : IServiceConfigurator {
  /// <summary>
  /// Configures the api-versioning to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration config) {
    services.AddApiVersioning(x => {
      // The default API version applied to services that do not have explicit versions
      x.DefaultApiVersion = ApiVersions.FromText(ApiVersions.V10);
      // Indicating whether a default version is assumed when a client does not provide an API version.
      x.AssumeDefaultVersionWhenUnspecified = true;
      // Indicating whether requests report the API version compatibility information in responses.
      x.ReportApiVersions = true;
      // The HTTP status code used for unsupported versions of an API.
      x.UnsupportedApiVersionStatusCode = 501;
      // The name associated with the API version route constrain
      //x.RouteConstraintName = "version";
    }).AddMvc().AddApiExplorer(
      // format the version as "'v'major[.minor][-status]"
      options => options.GroupNameFormat = "'v'VVV");
  }
}