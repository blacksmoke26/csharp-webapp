// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Movies.Minimal.Core.OpenApi;

internal sealed class BearerSecuritySchemeTransformer(
  IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer {
  public async Task TransformAsync(
    OpenApiDocument document,
    OpenApiDocumentTransformerContext context,
    CancellationToken cancellationToken
  ) {
    if (!(await authenticationSchemeProvider.GetAllSchemesAsync()).Any()) {
      return;
    }
    var requirements = new Dictionary<string, OpenApiSecurityScheme> {
      [JwtBearerDefaults.AuthenticationScheme] = new() {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme, // "bearer" refers to the header name here
        In = ParameterLocation.Header,
        BearerFormat = "Json Web Token",
        Description = "JWT Authorization header using the Bearer scheme"
      },
    };

    document.Components ??= new OpenApiComponents();
    document.Components.SecuritySchemes = requirements;

    var authOperations = document.Paths.Values
      .SelectMany(path => path.Operations)
      .Where(op => op.Value.Responses.ContainsKey("401"));

    OpenApiSecurityRequirement requirement = new() {
      [new OpenApiSecurityScheme {
        Reference = new () {
          Id = "Bearer",
          Type = ReferenceType.SecurityScheme
        },
        Scheme = "bearer", // "bearer" refers to the header name here
        In = ParameterLocation.Header,
        BearerFormat = "Json Web Token",
      }] = new List<string>()
    };

    // Apply it as a requirement for all operations
    foreach (var operation in authOperations) {
      operation.Value.Security.Add(requirement);
    }
  }
}