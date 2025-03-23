// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Api.Core.Constants;

public static class CachePolicies {
  public static class Movies {
    public const string Policy = "MoviePolicy";
    public const string Tag = "Movies";
    public static string[] VaryByQuery => ["title", "year", "sortBy", "page", "pageSize"];
  }
}