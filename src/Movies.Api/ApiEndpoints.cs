// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Api;

public static class ApiEndpoints {
  public const string ApiBase = "api/v{version:apiVersion}";

  public static class Movies {
    private const string Base = $"{ApiBase}/movies";

    public const string Create = Base;
    public const string Get = $"{Base}/{{idOrSlug}}";
    public const string GetAll = Base;
    public const string Update = $"{Base}/{{id:long}}";
    public const string Delete = $"{Base}/{{id:long}}";

    public const string Rating = $"{Base}/{{movieId:long}}/ratings";
    public const string DeleteRating = $"{Base}/{{movieId:long}}/ratings";
  }

  public static class Ratings {
    public const string Base = $"{ApiBase}/ratings";
    public const string GetUserRatings = $"{Base}/me";
  }

  public static class Auth {
    public const string Base = $"{ApiBase}/auth";
    public const string Login = $"{Base}/login";
    public const string Logout = $"{Base}/logout";
  }

  public static class Identity {
    public const string Base = $"{ApiBase}/identity";
    public const string Signup = $"{Base}/signup";
    public const string PasswordResetRequest = $"{Base}/password-reset-request";
    public const string PasswordReset = $"{Base}/password-reset";
  }

  public static class User {
    public const string Base = $"{ApiBase}/user";
    public const string Me = $"{Base}/me";
    public const string ChangePassword = $"{Base}/change-password";
  }
}