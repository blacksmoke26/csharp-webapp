// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.Security.Claims;

namespace Movies.Api.Core.Auth;

public class AuthRequirements : IAuthorizationHandler, IAuthorizationRequirement {
  public async Task HandleAsync(AuthorizationHandlerContext context) {
    await Task.Yield();
    
    if (context.User.HasClaim(x =>
          x.Type == ClaimTypes.Role
          && UserRole.GetRoles().Contains(x.Value))) {
      context.Succeed(this);
      return;
    }

    var httpContext = context.Resource as HttpContext;

    // No HTTP request
    if (httpContext is null) return;

    var apiKey = httpContext.GetApiKeyFromHeaders();

    if (apiKey is null) {
      context.Fail();
      return;
    }

    /*var identity = await idService.LoginWithApiKeyAsync(apiKey, new ApiKeyLoginOptions {
      IpAddress = httpContext.Connection.RemoteIpAddress?.ToString()
    });

    if (identity is null) {
      context.Fail();
      return;
    }*/
    
    //httpContext.GetIdentity().SetIdentity(identity);
    context.Succeed(this);
  }
}