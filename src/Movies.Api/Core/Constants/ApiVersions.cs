// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Api.Core.Constants;

public static class ApiVersions {
  public const string V10 = "1.0";
  public const string V11 = "1.1";

  public static ApiVersion FromText(string text)
    => ApiVersionParser.Default.Parse(text);
}