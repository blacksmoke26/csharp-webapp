// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Base;

[Description("Use to apply the sorting and pagination to the list of entities")]
public abstract record RequestQueryFetching {
  /// <summary> Sort order by, if the value starts with - then descending, otherwise ascending</summary>
  /// <remarks>Applicable for sorting orders by <c>field</c> name and <c>order</c></remarks>
  [JsonPropertyName("sortBy"), Description("Sort by Order")]
  public string? SortBy { get; set; } = null;

  /// <remarks>Applicable for <c>number</c> pagination</remarks>
  [JsonPropertyName("page"), Description("The page number to start from"), DefaultValue(1U)]
  public uint? Page { get; set; } = null;

  /// <remarks>Applicable for <c>number</c> pagination</remarks>
  [JsonPropertyName("pageSize"), Description("No. of record to be fetched on each page"), DefaultValue(10U)]
  public uint? PageSize { get; set; } = null;
}