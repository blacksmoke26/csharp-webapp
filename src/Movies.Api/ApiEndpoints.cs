// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api;

public static class ApiEndpoints {
  private const string ApiBase = "api";

  public static class Movies {
    private const string Base = $"{ApiBase}/movies";
    
    public const string Create = Base;
    public const string Get = $"{Base}/{{idOrSlug}}";
    public const string GetAll = Base;
    public const string Update = $"{Base}/{{id:guid}}";
    public const string Delete = $"{Base}/{{id:guid}}";
  }
}