// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi#use-operation-transformers

using CaseConverter;
using Dumpify;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Movies.Minimal.Core.OpenApi;

/// <summary>
/// The class used to transform the OpenApi Operation's Query parameter names
/// from `Pascalcase` into `Camelcase`
/// </summary>
internal sealed class ParametersCamelcaseOperationTransformer : IOpenApiOperationTransformer {
  public Task TransformAsync(
    OpenApiOperation operation, OpenApiOperationTransformerContext context,
    CancellationToken cancellationToken) {
    if (!(operation.Parameters?.Count > 0)) return Task.CompletedTask;

    foreach (var openApiParameter in operation.Parameters) {
      openApiParameter.Name = openApiParameter.Name.ToCamelCase();
    }

    return Task.CompletedTask;
  }
}