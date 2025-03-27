// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// Guide: https://medium.com/@baristanriverdi/building-dynamic-linq-expressions-meeting-ddd-and-specification-pattern-in-c-6a84c887a98a
// See: https://code-maze.com/dynamic-queries-expression-trees-csharp/
// See: https://sd.blackball.lv/en/articles/read/19155-expressions-in-net-a-practical-guide-to-system-linq-expressions

namespace Movies.Application.Core.Builders;

public static class ExpressionBuilder {
  public static Expression<Func<T, bool>> New<T>() {
    return x => true;
  }

  public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> expression) {
    return expression;
  }

  public static Expression<Func<T, bool>> And<T>(
    this Expression<Func<T, bool>> left,
    Expression<Func<T, bool>> right) {
    return Expression.Lambda<Func<T, bool>>(
      Expression.AndAlso(
        left.Body,
        Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
  }

  public static Expression<Func<T, bool>> Or<T>(
    this Expression<Func<T, bool>> left,
    Expression<Func<T, bool>> right) {
    return Expression.Lambda<Func<T, bool>>(
      Expression.OrElse(
        left.Body,
        Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
  }
}