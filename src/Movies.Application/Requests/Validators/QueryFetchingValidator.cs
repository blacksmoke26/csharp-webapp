// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Contracts.Base;

namespace Movies.Application.Requests.Validators;

/// <summary>
/// This class represents the validation specification of fields which are
/// required for fetching by sort name, sort order and the paginated results. 
/// </summary>
/// <typeparam name="T">The <c>RequestQueryFetching</c> DTO derived record</typeparam>
public class QueryFetchingValidator<T> : AbstractValidator<T> where T : RequestQueryFetching {
  /// <summary>List of supported sortBy fields name</summary>
  public IEnumerable<string> SortByFields { get; set; } = [];

  /// <summary>Default page sizes</summary>
  public IEnumerable<uint> DefaultPageSizes => [1, 5, 10, 20, 50, 100];
  
  /// <summary>Customized page sizes</summary>
  public IEnumerable<uint> PageSizes { get; set; }
  
  /// <summary>Ignores the <c>SortByFields</c> lookup validator</summary>
  public bool IgnoreSortByFields { get; set; } = false;
  
  /// <summary>Ignores the <c>PageSizes</c> lookup validator</summary>
  public bool IgnorePageSizes { get; set; } = false;

  public QueryFetchingValidator() {
    PageSizes = DefaultPageSizes;

    RuleFor(x => x.SortBy)
      .MinimumLength(2)
      .MaximumLength(20)
      .Must(ValidateSortBy)
      .WithMessage("The sort field should start with '-', '+' "
                   + "or a letter character (camel case or snake case only)")
      .Must(ValidateSupportedSortBy)
      .WithMessage("Unknown sort order field name given");

    // Number pagination specific
    RuleFor(x => x.Page)
      .GreaterThan(0U)
      .LessThanOrEqualTo(50_000U);

    RuleFor(x => x.PageSize)
      .GreaterThan(0U)
      .Must(ValidatePageSize)
      .WithMessage("Invalid or unsupported page size given");
  }

  /// <summary>
  /// Validates the <c>sortBy</c> value
  /// </summary>
  /// <param name="value">The input value</param>
  /// <returns>Whatever the sort by is valid or not</returns>
  private bool ValidateSortBy(string? value) {
    return value is null || StringHelper.IsSortOrderValue(value);
  }

  /// <summary>
  /// Validates the <c>sortBy</c> value supported by the list or not
  /// </summary>
  /// <param name="value">The input value</param>
  /// <returns>Whatever the sort by is valid or not</returns>
  private bool ValidateSupportedSortBy(string? value) {
    return value is null
           || IgnoreSortByFields || !SortByFields.Any()
           || StringHelper.HasSortOrderField(value, SortByFields);
  }

  /// <summary>
  /// Validates the <c>pageSize</c> value
  /// </summary>
  /// <param name="value">The input value</param>
  /// <returns>Whatever the sort by is valid or not</returns>
  private bool ValidatePageSize(uint? value) {
    return value is null
           || IgnorePageSizes || !PageSizes.Any()
           || PageSizes.Contains((uint)value);
  }
}