// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Core.Swagger.Filters;

internal class ObsoleteOperationFilter : IOperationFilter {
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    operation.Description = operation.Deprecated
      ? context.MethodInfo.GetCustomAttribute<ObsoleteAttribute>()?.Message
      : operation.Description;
  }
}