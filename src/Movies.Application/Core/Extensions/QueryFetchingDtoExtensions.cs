// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Base;

namespace Movies.Application.Core.Extensions;

public static class QueryFetchingDtoExtensions {
  /// <summary>Returns the supplied sort by field name</summary>
  /// <remarks>In case of empty or null, the default field name will be returned</remarks>
  /// <param name="dto">The input DTO</param>
  /// <param name="defaultField">The default field name if there is none</param>
  /// <returns>The sort by field name</returns>
  public static string GetSortField(this RequestQueryFetching dto, string defaultField = "") {
    return string.IsNullOrWhiteSpace(dto.SortBy)
      ? defaultField
      : StringHelper.UnescapeSortOrder(dto.SortBy);
  }

  /// <summary>Returns the supplied sort by order type</summary>
  /// <param name="dto">The input DTO</param>
  /// <param name="defaultOrder">The default sort order if there is none</param>
  /// <returns>The sort by order</returns>
  public static SortOrder GetSortOrder(
    this RequestQueryFetching dto, SortOrder defaultOrder = SortOrder.Ascending)
    => dto.SortBy is null ? defaultOrder : StringHelper.GetSortByFrom(dto.SortBy);

  /// <summary>Returns the supplied page size</summary>
  /// <remarks>In case of empty or null, the default page size will be returned</remarks>
  /// <param name="dto">The input DTO</param>
  /// <param name="defaultPageSize">The default page size if there is none</param>
  /// <returns>The page size</returns>
  public static int GetPageSize(this RequestQueryFetching dto, uint defaultPageSize = 15U)
    => (int)(dto.PageSize ?? defaultPageSize);

  /// <summary>Returns the supplied page number</summary>
  /// <remarks>In case of empty or null, the default page number will be returned</remarks>
  /// <param name="dto">The input DTO</param>
  /// <param name="defaultPage">The default page number if there is none</param>
  /// <returns>The page number</returns>
  public static int GetPage(this RequestQueryFetching dto, uint defaultPage = 1U)
    => (int)(dto.Page ?? defaultPage);

  /// <summary>Returns the supplied page number</summary>
  /// <remarks>In case of empty or null, the default page number will be returned</remarks>
  /// <param name="dto">The input DTO</param>
  /// <returns>The page number</returns>
  public static PaginatorOptions GetPageOptions(this RequestQueryFetching dto)
    => new() { Page = dto.GetPage(), PageSize = dto.GetPageSize() };
}