// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

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
      .WithName(Name)
      .WithSummary("Signup")
      .WithDescription("Creates a user account")
      .WithTags("Identity")
      .WithVersioning(ApiVersions.V10)
      .Produces<SuccessWithMessageResponse>(StatusCodes.Status201Created)
      .Produces<OperationFailureResponse>(StatusCodes.Status400BadRequest)
      .Produces<ValidationFailureResponse>(StatusCodes.Status422UnprocessableEntity);

    return app;
  }
}