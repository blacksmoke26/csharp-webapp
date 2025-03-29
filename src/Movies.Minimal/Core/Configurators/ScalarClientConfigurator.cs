// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Scalar: https://github.com/scalar/scalar/blob/main/documentation/integrations/dotnet.md

using Movies.Api.Core.Interfaces;
using Scalar.AspNetCore;

namespace Movies.Minimal.Core.Configurators;

/// <summary>
/// This configurator class is used to configure the <see href="https://scalar.com/">Scalar</see>
/// API client for the application 
/// </summary>
abstract class ScalarClientConfigurator : IApplicationConfigurator {
  public static void Use(WebApplication app) {
    var versions = ApiVersions.VersionsList
      .Select(x => x.ToString(ApiVersions.VersionFormat)).ToArray();

    app.MapScalarApiReference(opt => {
      opt.AddDocuments(versions);
      opt.WithPreferredScheme("Bearer");
      opt.WithTestRequestButton(true);
      opt.Title = $"Movies: API Reference ({app.Environment.EnvironmentName})";
      opt.Theme = ScalarTheme.Purple;
      opt.HideClientButton = true;
      opt.HideDownloadButton = true;
    });
  }
}