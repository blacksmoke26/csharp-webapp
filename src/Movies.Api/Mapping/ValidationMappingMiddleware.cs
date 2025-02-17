// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using FluentValidation;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

/// <summary>
/// Server middleware to normalize the validation errors caused by FluentValidation
/// </summary>
/// <param name="next">The middleware delegate</param>
public class ValidationMappingMiddleware(RequestDelegate next) {
  public async Task InvokeAsync(HttpContext content) {
    try {
      await next(content);
    }
    catch (ValidationException ex) {
      content.Response.StatusCode = 400;
      ValidationFailureResponse validationFailureResponse = new() {
        Errors = ex.Errors.Select(x => new ValidationResponse {
          PropertyName = x.PropertyName,
          Message = x.ErrorMessage,
        })
      };

      await content.Response.WriteAsJsonAsync(validationFailureResponse);
    }
  }
}