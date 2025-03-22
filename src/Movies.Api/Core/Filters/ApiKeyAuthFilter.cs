// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/
// See: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/claims

using Microsoft.AspNetCore.Mvc.Filters;
using Movies.Api.Core.Auth;
using Movies.Application.Config;

namespace Movies.Api.Core.Filters;

public class ApiKeyAuthFilter(AppConfiguration config) : IAsyncAuthorizationFilter {
  public Task OnAuthorizationAsync(AuthorizationFilterContext context) {
    if (!context.HttpContext.Request.Headers.TryGetValue(
          AuthConstants.ApiKeyHeaderName, out var apiKey)) {
      throw ErrorHelper.CustomError("No API Key was provided", ErrorCodes.Unauthorized);
    }

    var userId = config.AuthConfig().ApiKeys
      .Where(x => x.Value == apiKey)
      .Select(x => x.Key).FirstOrDefault();

    if (userId == 0) {
      throw ErrorHelper.CustomError("The API Key is invalid", ErrorCodes.Forbidden);
    }

    return Task.CompletedTask;
  }
}