// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format

using Asp.Versioning;
using Asp.Versioning.Builder;

namespace Movies.Minimal.Core.Constants;

internal static class ApiVersions {
  public const double V10 = 1.0;
  public const double V11 = 1.1;

  public const string VersionFormat = "'v'VVV";
  public const string VersionFormatDecimal = "VV";

  /// <summary>
  /// Parse the API version from text
  /// </summary>
  public static ApiVersion FromText(string text)
    => ApiVersionParser.Default.Parse(text);

  /// <summary>
  /// Supported API versions
  /// </summary>
  public static IEnumerable<ApiVersion> VersionsList => [
    new(V10), new(V11)
  ];

  /// <summary>
  /// Supported API versions set
  /// </summary>
  public static ApiVersionSet VersionSet { get; private set; } = null!;

  /// <summary>
  /// Initializes a <see cref="Asp.Versioning.Builder.ApiVersionSet">ApiVersionSet</see> for the endpoints.
  /// </summary>
  /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder"/>.</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Routing.IEndpointRouteBuilder"/>.</returns>
  public static WebApplication CreateApiVersionSet(this WebApplication app) {
    var versionSet = app.NewApiVersionSet();

    foreach (var version in VersionsList) {
      versionSet.HasApiVersion(version);
    }

    VersionSet = versionSet.Build();

    return app;
  }
}