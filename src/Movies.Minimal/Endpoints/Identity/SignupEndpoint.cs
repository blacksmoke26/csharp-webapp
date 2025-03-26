// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Movies.Api;

namespace Movies.Minimal.Endpoints.Identity;

public static class SignupEndpoint {
  public const string Name = "SignupIdentity";

  public static IEndpointRouteBuilder MapSignupIdentity(this IEndpointRouteBuilder app) {
    app.MapPost(ApiEndpoints.Identity.Signup, async (
        UserSignupPayload body,
        UserService userService, CancellationToken token
      ) => {
        await userService.CreateAsync(new() {
          Email = body.Email,
          Password = body.Password,
          FirstName = body.FirstName,
          LastName = body.LastName,
        }, token);

        return TypedResults.Json(
          ResponseHelper.SuccessWithMessage(
            "You account has been created. Please check your inbox for verification email"),
          statusCode: StatusCodes.Status201Created);
      })
      .WithName(Name);
    
    return app;
  }
}