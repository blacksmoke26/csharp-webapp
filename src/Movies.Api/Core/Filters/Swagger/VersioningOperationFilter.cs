﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Core.Filters.Swagger;

public class VersioningOperationFilter : IOperationFilter {
  /// <inheritdoc/>
  public void Apply(OpenApiOperation operation, OperationFilterContext context) {
    var apiDescription = context.ApiDescription;
    operation.Deprecated |= apiDescription.IsDeprecated();

    foreach (var responseType in context.ApiDescription.SupportedResponseTypes) {
      var responseKey = responseType.IsDefaultResponse
        ? "default"
        : responseType.StatusCode.ToString();

      var response = operation.Responses[responseKey];

      foreach (var contentType in response.Content.Keys) {
        if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType)) {
          response.Content.Remove(contentType);
        }
      }
    }

    if (operation.Parameters == null) {
      return;
    }

    foreach (var parameter in operation.Parameters) {
      var description = apiDescription.ParameterDescriptions
        .First(p => p.Name == parameter.Name);

      parameter.Description ??= description.ModelMetadata.Description;

      if (parameter.Schema.Default == null && description.DefaultValue != null) {
        var json = JsonConvert.SerializeObject(
          description.DefaultValue, description.ModelMetadata.ModelType, Formatting.None, new JsonSerializerSettings {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
          });

        parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
      }

      parameter.Required |= description.IsRequired;
    }
  }
}