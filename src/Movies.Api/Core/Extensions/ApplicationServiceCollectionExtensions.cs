// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.Reflection;
using Dumpify;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Movies.Application.Config;
using Movies.Application.Objects;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace Movies.Api.Core.Extensions;

/// <summary>
/// Represents as the part of dependency injection to register the dependencies inside the application
/// </summary>
public static class ApplicationServiceCollectionExtensions {
  /// <summary>
  /// Registers application level services 
  /// </summary>
  /// <param name="services">ServiceCollection instance</param>
  /// <param name="configuration">Application configuration</param>
  /// <returns>The updated service collection instance</returns>
  public static IServiceCollection InitBootstrapper(
    this IServiceCollection services, IConfiguration configuration
  ) {
    var config = new AppConfiguration(configuration);

    services.AddSingleton<AppConfiguration>(_ => config);

    #region JWT Authentication / Authorization

    services.AddAuthentication(x => {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => {
      x.TokenValidationParameters = new TokenValidationParameters() {
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(config.JwtConfig().Key)
        ),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config.JwtConfig().Issuer,
        ValidAudience = config.JwtConfig().Audience,
        ValidateIssuer = true,
        ValidateAudience = true
      };
    });

    services.AddAuthorization(x => {
      // Policy Type: Admin
      x.AddPolicy(AuthPolicies.AdminPolicy, p
        => p.RequireRole(UserRole.Admin));

      // Policy Type: User
      x.AddPolicy(AuthPolicies.UserPolicy, p
        => p.RequireRole(UserRole.User));

      // Policy Type: Trusted
      x.AddPolicy(AuthPolicies.AuthPolicy, p
        => p.RequireRole(UserRole.Admin, UserRole.User));
    });

    services.AddSingleton<UserIdentity>();
    services.AddSingleton<AuthService>(_ => new AuthService(config.JwtConfig()));

    #endregion

    services.AddFluentValidationAutoValidation(c => {
      // Disable the built-in .NET model (data annotations) validation.
      c.DisableBuiltInModelValidation = true;

      // Only validate controllers decorated with the `AutoValidation` attribute.
      c.ValidationStrategy = ValidationStrategy.Annotations;

      // Enable validation for parameters bound from `BindingSource.Body` binding sources.
      c.EnableBodyBindingSourceAutomaticValidation = true;

      // Enable validation for parameters bound from `BindingSource.Form` binding sources.
      c.EnableFormBindingSourceAutomaticValidation = true;

      // Enable validation for parameters bound from `BindingSource.Query` binding sources.
      c.EnableQueryBindingSourceAutomaticValidation = true;

      // Enable validation for parameters bound from `BindingSource.Path` binding sources.
      c.EnablePathBindingSourceAutomaticValidation = true;

      // Enable validation for parameters bound from 'BindingSource.Custom' binding sources.
      c.EnableCustomBindingSourceAutomaticValidation = true;

      // Replace the default result factory with a custom implementation.
      //c.OverrideDefaultResultFactoryWith<CustomResultFactory>();
    });


    // Add services to the container.
    services.AddControllers();
    services.AddProblemDetails();
    //services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();

    services.AddEndpointsApiExplorer();

    services.AddApplication();
    services.AddExceptionHandler(x => { x.ExceptionHandlingPath.Value.Dump(); });
    services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
    services.AddDatabase(config.Config["Database:ConnectionString"]!);

    return services;
  }
}

public class CustomResultFactory : IFluentValidationAutoValidationResultFactory {
  public IActionResult CreateActionResult(ActionExecutingContext context,
    ValidationProblemDetails? validationProblemDetails) {
    validationProblemDetails.Title.Dump();
    return new BadRequestObjectResult(new
      { Title = "Validation errors" });
  }
}