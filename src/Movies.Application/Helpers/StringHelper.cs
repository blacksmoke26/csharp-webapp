// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using CaseConverter;

namespace Movies.Application.Helpers;

public static class StringHelper {
  /// <summary>
  /// Generates slug from the given values
  /// </summary>
  /// <param name="values">The stringy values</param>
  /// <returns>The computed sluggish value</returns>
  public static string GenerateSlug(params object?[] values)
    => string.Concat(values).ToLower().ToKebabCase();
}