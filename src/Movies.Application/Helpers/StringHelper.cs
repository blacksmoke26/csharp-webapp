// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Text.RegularExpressions;
using CaseConverter;
using Movies.Application.Core.Extensions;

namespace Movies.Application.Helpers;

public static class StringHelper {
  /// <summary>
  /// Generates slug from the given values
  /// </summary>
  /// <param name="values">The stringy values</param>
  /// <returns>The computed sluggish value</returns>
  public static string GenerateSlug(params object?[] values)
    => string.Concat(values).ToLower().ToKebabCase();

  /// <summary>
  /// Validates the given value is a valid sort order value or not.
  /// </summary>
  /// <param name="value">The value to which contains sort order expression</param>
  /// <returns>Whatever the sort order expression is valid or not</returns>
  /// <example>
  /// Expression samples.
  ///<code>
  /// +year (Lower cased with ascending order)
  /// -yearName // camel cased with descending order
  /// createdDate // optional symbol (camel case)
  /// year_name // optional symbol (snake case)
  /// </code>
  /// </example>
  ///<remarks><b>Note:</b> The beginning sorting symbols (<c>-</c> and <c>+</c>) are totally optional.</remarks>
  public static bool IsSortOrderValue(string? value) {
    return !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(
      value, "^([-+])?[a-z]([a-z0-9])+(_?[a-z0-9]+)*$", RegexOptions.IgnoreCase);
  }

  /// <summary>
  /// Checks if the specified sort order name exists in the given list
  /// </summary>
  /// <param name="value">The value to which contains sort order expression</param>
  /// <param name="fieldsList">The sort orders list to verify</param>
  /// <returns>Whatever the sort order exists or not</returns>
  public static bool HasSortOrderField(string? value, IEnumerable<string> fieldsList) {
    return IsSortOrderValue(value) && fieldsList.Contains(UnescapeSortOrder(value));
  }

  /// <summary>
  /// Omits the <c>-</c> and <c>+</c> from the starting of the given value
  /// </summary>
  /// <param name="value">The value to which contains sort order expression</param>
  /// <returns>The sort field name</returns>
  public static string UnescapeSortOrder(string? value)
    => value?.Trim('-', '+').ToCamelCase() ?? string.Empty;

  /// <summary>
  /// Return the sort order by type from the specified value
  /// </summary>
  /// <param name="value">The value to which contains sort order expression</param>
  /// <returns>The sort by type from value</returns>
  public static SortOrder GetSortByFrom(string value)
    => value.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending;
}