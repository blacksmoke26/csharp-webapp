// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Asp.Versioning;
using Movies.Minimal.Core.OpenApi;

namespace Movies.Minimal.Core.Extensions;

public static class RouteHandlerBuilderExtensions {
  /// <summary>
  /// Mark the endpoint as deprecated
  /// </summary>
  /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</param>
  /// <param name="description">The information to display regarding alternate endpoint</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</returns>
  /// <example><code>app.mapGet("/api/..", () => {})
  ///   .IsDeprecated("Please use the <code>loginUser</code> endpoint instead");</code></example>
  public static RouteHandlerBuilder IsDeprecated(this RouteHandlerBuilder builder, string? description = null) {
    return builder.WithMetadata(new DeprecatedOpenApiFlag { Description = description });
  }

  /// <summary>
  /// Indicates that the endpoint requires authorization with contain <see cref="StatusCodes.Status401Unauthorized">401</see> response.
  /// </summary>
  /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</param>
  /// <param name="policyNames">A collection of <see cref="AuthPolicies">policy names</see>. If empty, the default authorization policy will be used.</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</returns>
  public static RouteHandlerBuilder WithAuthorization(this RouteHandlerBuilder builder, params string[] policyNames) {
    return builder
      .RequireAuthorization(policyNames)
      .Produces(StatusCodes.Status401Unauthorized);
  }

  /// <summary>
  /// Enables the API versioning support for the endpoint
  /// </summary>
  /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</param>
  /// <param name="versions">The supported <see cref="Asp.Versioning.ApiVersion">API versions</see> by this endpoint.</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</returns>
  public static RouteHandlerBuilder WithVersioning(this RouteHandlerBuilder builder, params double[] versions) {
    return WithVersioning(builder, versions.Select(version
      => new ApiVersion(version)).ToArray());
  }

  /// <summary>
  /// Enables the API versioning support for the endpoint
  /// </summary>
  /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</param>
  /// <param name="versions">The supported <see cref="Asp.Versioning.ApiVersion">API versions</see> by this endpoint.</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</returns>
  public static RouteHandlerBuilder WithVersioning(this RouteHandlerBuilder builder, params string[] versions) {
    return WithVersioning(builder, versions.Select(version
      => ApiVersionParser.Default.Parse(version)).ToArray());
  }

  /// <summary>
  /// Enables the API versioning support for the endpoint
  /// </summary>
  /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</param>
  /// <param name="versions">The supported <see cref="Asp.Versioning.ApiVersion">API versions</see> by this endpoint.</param>
  /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IEndpointConventionBuilder" />.</returns>
  public static RouteHandlerBuilder WithVersioning(this RouteHandlerBuilder builder, params ApiVersion[] versions) {
    builder.WithApiVersionSet(ApiVersions.VersionSet);

    foreach (var version in versions) {
      builder.HasApiVersion(version);
    }

    return builder;
  }
}