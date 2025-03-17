// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Base;

public abstract record RequestQueryFetching {
  /// <summary> Sort order by, if the value starts with - then descending, otherwise ascending</summary>
  /// <remarks>Applicable for sorting orders by <c>field</c> name and <c>order</c></remarks>
  public string? SortBy { get; set; } = null;
  
  /// <summary>The page no to fetch results from</summary>
  /// <remarks>Applicable for <c>number</c> pagination</remarks>
  public uint? Page { get; set; } = null;

  /// <summary>No. of record to be fetched on each request</summary>
  /// <remarks>Applicable for <c>number</c> pagination</remarks>
  public uint? PageSize { get; set; } = null;
}