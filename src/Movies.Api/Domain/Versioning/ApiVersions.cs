// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Api.Domain.Versioning;

public static class ApiVersions {
  public const string V10 = "1.0";
  public const string V11 = "1.1";

  public static ApiVersion FromText(string text)
    => ApiVersionParser.Default.Parse(text);
}