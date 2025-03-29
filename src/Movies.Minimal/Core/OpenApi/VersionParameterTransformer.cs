// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi#use-operation-transformers

using Asp.Versioning;
using CaseConverter;
using Dumpify;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Movies.Minimal.Core.OpenApi;

/// <summary>
/// The class used to transform the OpenApi Operation's Query parameter names
/// from `Pascalcase` into `Camelcase`
/// </summary>
internal sealed class VersionParameterTransformer : IOpenApiDocumentTransformer {
  public Task TransformAsync(
    OpenApiDocument document,
    OpenApiDocumentTransformerContext context,
    CancellationToken cancellationToken
  ) {
    foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations)) {
      operation.Value.Parameters ??= new List<OpenApiParameter>();
      operation.Value.Parameters.Insert(0, new OpenApiParameter {
        Name = "version",
        In = ParameterLocation.Path,
        Description = "The requested API version",
        Required = true,
        Schema = new OpenApiSchema {
          Type = "string",
          Default = new OpenApiString(document.Info.Version)
        }
      });
    }

    return Task.CompletedTask;
  }
}