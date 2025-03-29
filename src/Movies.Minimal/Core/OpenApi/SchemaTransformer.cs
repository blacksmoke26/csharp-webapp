// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi#use-schema-transformers

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Movies.Minimal.Core.OpenApi;

public class SchemaTransformer : IOpenApiSchemaTransformer {
  /// <inheritdoc/>
  public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
    CancellationToken cancellationToken) {
    
    if (context.JsonTypeInfo.Type == typeof(decimal)) {
      schema.Format = "decimal";
    }

    return Task.CompletedTask;
  }
}