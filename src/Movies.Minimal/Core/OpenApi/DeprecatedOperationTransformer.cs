// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/customize-openapi#use-schema-transformers
// See also: https://dev.to/bytehide/how-to-integrate-openapi-into-your-aspnet-core-projects-with-net-9-399a
// Repo: https://github.dev/royberris/Berris.OpenApi.Deprecated

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Movies.Minimal.Core.OpenApi;

internal sealed class DeprecatedOperationTransformer : IOpenApiOperationTransformer {
  public Task TransformAsync(
    OpenApiOperation operation, OpenApiOperationTransformerContext context,
    CancellationToken cancellationToken) {
    var deprecatedFlag = context.Description.ActionDescriptor.EndpointMetadata
      .OfType<DeprecatedOpenApiFlag>().FirstOrDefault();

    if (deprecatedFlag is not null) {
      operation.Deprecated = true;
      operation.Description =
        $"<s><em>{operation.Description}</em></s>" +
        $"<p><b>Note:</b> {deprecatedFlag.Description}</p>";
    }

    return Task.CompletedTask;
  }
}

internal record DeprecatedOpenApiFlag {
  public string? Description { get; set; } = null;
}