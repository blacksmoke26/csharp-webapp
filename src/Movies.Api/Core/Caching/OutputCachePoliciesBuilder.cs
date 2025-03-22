// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Microsoft.AspNetCore.OutputCaching;

namespace Movies.Api.Core.Caching;

public static class OutputCachePoliciesBuilder {
  /// <summary>
  /// Builds the output caching policies
  /// </summary>
  /// <param name="opt">The <see cref="Microsoft.AspNetCore.OutputCaching.OutputCacheOptions">OutputCacheOptions</see> options</param>
  public static void Build(OutputCacheOptions opt) {
    // Add `Movies` policy
    opt.AddPolicy(CachePolicies.Movies.Policy, c => {
      c.Cache()
        .Expire(TimeSpan.FromMinutes(1))
        .SetVaryByQuery(CachePolicies.Movies.VaryByQuery)
        .Tag(CachePolicies.Movies.Policy);
    });
  }
}