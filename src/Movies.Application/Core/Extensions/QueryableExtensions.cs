// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/
// See: https://blog.jetbrains.com/dotnet/2024/06/26/how-where-conditions-work-in-entity-framework-core/
// See: https://medium.com/@baristanriverdi/building-dynamic-linq-expressions-meeting-ddd-and-specification-pattern-in-c-6a84c887a98a

namespace Movies.Application.Core.Extensions;

public static class QueryableExtensions {
  /// <summary>
  /// Patternize the value using '%' simple for *LIKE clauses
  /// </summary>
  /// <param name="value">The value</param>
  /// <param name="likePattern">Pattern type</param>
  /// <returns>The patternize value</returns>
  private static string PatternizeLikeValue(string value, FilterLikePattern likePattern) {
    return likePattern switch {
      FilterLikePattern.Both => string.Concat('%', value, '%'),
      FilterLikePattern.LeftOnly => string.Concat('%', value),
      FilterLikePattern.RightOnly => string.Concat(value, '%'),
      FilterLikePattern.None => value,
      _ => throw new ArgumentOutOfRangeException(nameof(likePattern), likePattern, null)
    };
  }

  /// <summary>A sequence of values based on a predicate only runs when the condition is true</summary>
  /// <remarks>This query will be ignored if the condition is false.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="condition">The conditional value to check</param>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> If<TSource>(
    this IQueryable<TSource> source, bool condition,
    Expression<Func<TSource, bool>> predicate) {
    return condition ? source.Where(predicate) : source;
  }

  /// <summary>A sequence of values based on a predicates upon true or false condition</summary>
  /// <remarks>This query will be ignored if the condition is false.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="condition">The conditional value to check</param>
  /// <param name="ifPredicate">An if based function to perform a query on sequence.</param>
  /// <param name="elsePredicate">An Else based function to perform a query on sequence.</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> IfOrElse<TSource>(
    this IQueryable<TSource> source,
    bool condition,
    Expression<Func<TSource, bool>> ifPredicate,
    Expression<Func<TSource, bool>> elsePredicate) {
    return condition
      ? source.Where(ifPredicate)
      : source.Where(elsePredicate);
  }

  /// <summary>A queryable sequence of values based on a predicates upon true or false condition</summary>
  /// <remarks>This query will be ignored if the condition is false.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="condition">The conditional value to check</param>
  /// <param name="ifQueryable">An if based function to perform a query on sequence.</param>
  /// <param name="elseQueryable">An Else based function to perform a query on sequence.</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> IfOrElse<TSource>(
    this IQueryable<TSource> source,
    bool condition,
    Func<IQueryable<TSource>, IQueryable<TSource>> ifQueryable,
    Func<IQueryable<TSource>, IQueryable<TSource>> elseQueryable) {
    return condition
      ? ifQueryable(source)
      : elseQueryable(source);
  }

  /// <summary>Adds the sequence within the current queryable sequence</summary>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="queryable">The input sequence. Upon nullable, it will be ignored</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> AddQuery<TSource>(
    this IQueryable<TSource> source,
    Func<IQueryable<TSource>, IQueryable<TSource>>? queryable) {
    return queryable?.Invoke(source) ?? source;
  }

  /// <summary>Sorts the elements of a sequence in ascending or descending order according to a key.</summary>
  /// <remarks>This query will be ignored if the condition is false.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="sortOrder">The sort order type</param>
  /// <param name="keySelector">A function to extract a key from an element</param>
  /// <typeparam name="TSource">The type of the elements of source.</typeparam>
  /// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> SortBy<TSource, TKey>(
    this IQueryable<TSource> source, SortOrder sortOrder,
    Expression<Func<TSource, TKey>> keySelector) {
    return sortOrder switch {
      SortOrder.Ascending => source.OrderBy(keySelector),
      SortOrder.Descending => source.OrderByDescending(keySelector),
      _ => throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, null)
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a predicates.
  /// </summary>
  /// <remarks>This query will be ignored if the value is either null, empty or contain whitespaces only.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="value">The value to match against</param>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  /// <example>
  /// The following example shows the quick usage of this method:
  /// <code>await context.Posts
  /// .Filter(dto.Year, x => x.YearOfRelease.Equals(dto.Year!))
  /// .ListAsync();</code>
  /// </example>
  public static IQueryable<TSource> Filter<TSource>(
    this IQueryable<TSource> source, object? value,
    Expression<Func<TSource, bool>> predicate) {
    return value is null ? source : source.Where(predicate);
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A queryable sequence of values based on a predicates.
  /// </summary>
  /// <remarks>This query will be ignored if the value is either null, empty or contain whitespaces only.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="value">The value to match against</param>
  /// <param name="queryable">A queryable function to perform queryable functions.</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  /// <example>
  /// The following example shows the quick usage of this method:
  /// <code>await context.Posts
  /// .Filter(dto.Year, q
  ///   => q
  ///    .Where(x.YearOfRelease.Equals(dto.Year!))
  ///    .OrderBy(x.CreatedAt)
  /// .ListAsync();</code>
  /// </example>
  public static IQueryable<TSource> Filter<TSource>(
    this IQueryable<TSource> source, object? value,
    Func<IQueryable<TSource>, IQueryable<TSource>> queryable) {
    return value is null ? source : queryable.Invoke(source);
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a string value.
  /// which filters the sequence of values against the shadow-property and the value.
  /// <p><b>Note:</b> This query will be ignored if the value is either <c>null</c>, <c>empty</c> and contains <c>only whitespaces</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    string? value, FilterComparisonEqual comparison) {
    if (string.IsNullOrWhiteSpace(value)) return source;
    return comparison switch {
      FilterComparisonEqual.NotEqual => source.Where(x => EF.Property<string>(x!, propertyName) != value),
      FilterComparisonEqual.Equal => source.Where(x => EF.Property<string>(x!, propertyName) == value),
      _ => throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null)
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a float value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    short? value, FilterComparison comparison) {
    if (value is null) return source;
    return comparison switch {
      FilterComparison.NotEqual => source.Where(x => EF.Property<short>(x!, propertyName) != value),
      FilterComparison.GreaterThan => source.Where(x => EF.Property<short>(x!, propertyName) > value),
      FilterComparison.GreaterThanOrEqual => source.Where(x => EF.Property<short>(x!, propertyName) >= value),
      FilterComparison.LessThan => source.Where(x => EF.Property<short>(x!, propertyName) < value),
      FilterComparison.LessTheaOrEqual => source.Where(x => EF.Property<short>(x!, propertyName) <= value),
      _ => source.Where(x => EF.Property<short>(x!, propertyName) == value),
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and an <c>integer</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    int? value, FilterComparison comparison) {
    if (value is null) return source;
    return comparison switch {
      FilterComparison.NotEqual => source.Where(x => EF.Property<int>(x!, propertyName) != value),
      FilterComparison.GreaterThan => source.Where(x => EF.Property<int>(x!, propertyName) > value),
      FilterComparison.GreaterThanOrEqual => source.Where(x => EF.Property<int>(x!, propertyName) >= value),
      FilterComparison.LessThan => source.Where(x => EF.Property<int>(x!, propertyName) < value),
      FilterComparison.LessTheaOrEqual => source.Where(x => EF.Property<int>(x!, propertyName) <= value),
      _ => source.Where(x => EF.Property<int>(x!, propertyName) == value),
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a <c>long</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    long? value, FilterComparison comparison) {
    if (value is null) return source;
    return comparison switch {
      FilterComparison.NotEqual => source.Where(x => EF.Property<long>(x!, propertyName) != value),
      FilterComparison.GreaterThan => source.Where(x => EF.Property<long>(x!, propertyName) > value),
      FilterComparison.GreaterThanOrEqual => source.Where(x => EF.Property<long>(x!, propertyName) >= value),
      FilterComparison.LessThan => source.Where(x => EF.Property<long>(x!, propertyName) < value),
      FilterComparison.LessTheaOrEqual => source.Where(x => EF.Property<long>(x!, propertyName) <= value),
      _ => source.Where(x => EF.Property<long>(x!, propertyName) == value),
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a <c>decimal</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    decimal? value, FilterComparison comparison) {
    if (value is null) return source;
    return comparison switch {
      FilterComparison.NotEqual => source.Where(x => EF.Property<decimal>(x!, propertyName) != value),
      FilterComparison.GreaterThan => source.Where(x => EF.Property<decimal>(x!, propertyName) > value),
      FilterComparison.GreaterThanOrEqual => source.Where(x => EF.Property<decimal>(x!, propertyName) >= value),
      FilterComparison.LessThan => source.Where(x => EF.Property<decimal>(x!, propertyName) < value),
      FilterComparison.LessTheaOrEqual => source.Where(x => EF.Property<decimal>(x!, propertyName) <= value),
      _ => source.Where(x => EF.Property<decimal>(x!, propertyName) == value),
    };
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a <c>float</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    float? value, FilterComparison comparison) {
    return FilterCompare(source, propertyName, (decimal?)value, comparison);
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a <c>double</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    double? value, FilterComparison comparison) {
    return FilterCompare(source, propertyName, (decimal?)value, comparison);
  }

  /// <summary>
  /// An optional version of <b>Queryable.Where()</b>, A sequence of values based on a shadow-property and a <c>boolean</c> value.
  /// <p><b>Note:</b> This query will be ignored if the value is <c>null</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="comparison">The comparison type</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  public static IQueryable<TSource> FilterCompare<TSource>(
    this IQueryable<TSource> source,
    string propertyName,
    bool? value, FilterComparisonEqual comparison) {
    if (value is null) return source;
    return comparison switch {
      FilterComparisonEqual.NotEqual => source.Where(x => EF.Property<bool>(x!, propertyName) != value),
      _ => source.Where(x => EF.Property<bool>(x!, propertyName) == value),
    };
  }

  /// <summary>
  /// Filters a sequence of values based on a <b>ILIKE</b> clause. This method is case-insensitive.
  /// <p><b>Note:</b> This query will be ignored if the value is either <c>null</c>, <c>empty</c> or contain <c>whitespaces only</c>.</p>
  /// </summary>
  /// <remarks>The <b>property name</b> is a
  /// <see href="https://learn.microsoft.com/en-us/ef/core/modeling/shadow-properties">shadow-property</see>
  /// and will be translated by the LINQ query.</remarks>
  /// <param name="source">A sequence to convert.</param>
  /// <param name="propertyName">The shadow property name</param>
  /// <param name="value">The value to match against</param>
  /// <param name="pattern"><b>%</b> pattern to append around the value</param>
  /// <typeparam name="TSource">The entity object</typeparam>
  /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that represents the input sequence.</returns>
  /// <example>
  /// The following example shows the quick usage of this method:
  /// <code>await context.Posts
  /// .Where(x => x.UserId == 6)
  /// .FilterILike("Title", "Heading") // "Heading" became "%Heading%"
  /// .ListAsync();</code>
  /// </example>
  public static IQueryable<TSource> FilterILike<TSource>(
    this IQueryable<TSource> source, string propertyName, string? value,
    FilterLikePattern pattern = FilterLikePattern.Both) {
    if (string.IsNullOrWhiteSpace(value)) return source;
    return source.Where(x
      => EF.Functions.ILike(
        EF.Property<string>(x!, propertyName), PatternizeLikeValue(value, pattern)
      ));
  }
}

public enum FilterLikePattern {
  Both,
  LeftOnly,
  RightOnly,
  None
}

public enum FilterComparison {
  Equal,
  NotEqual,
  GreaterThan,
  GreaterThanOrEqual,
  LessThan,
  LessTheaOrEqual
}

public enum FilterComparisonEqual {
  Equal,
  NotEqual
}

public enum SortOrder {
  Ascending,
  Descending
}