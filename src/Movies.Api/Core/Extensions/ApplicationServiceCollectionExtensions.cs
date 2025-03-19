// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Core.Swagger;
using Movies.Application.Config;
using Newtonsoft.Json.Serialization;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

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

    services.AddVersioning();

    AddAuthentication(services, config);

    AddErrorHandlers(services);

    AddControllers(services);

    services.AddSwagger();

    services.AddApplication();
    services.AddDatabase(config);

    return services;
  }

  /// <summary>
  /// Adds MVC controllers to the service collection
  /// </summary>
  /// <param name="services">The ServiceCollection instance</param>
  public static void AddControllers(IServiceCollection services) {
    // Add services to the container.
    services.AddControllers()
      .AddNewtonsoftJson(options => {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      });
  }

  /// <summary>
  /// Adds authentication and authorization to the service collection
  /// </summary>
  /// <param name="services">The ServiceCollection instance</param>
  /// <param name="config">The AppConfiguration instance</param>
  public static void AddAuthentication(IServiceCollection services, AppConfiguration config) {
    #region JWT Authentication / Authorization

    services.AddAuthentication(x => {
      x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x => {
      x.TokenValidationParameters = new TokenValidationParameters() {
        IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(config.JwtConfig().Key!)
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

    services.AddSingleton<AuthService>(_ => new AuthService(config.JwtConfig()));

    #endregion
  }

  /// <summary>
  /// Adds error handlers / validation error transformers
  /// </summary>
  /// <param name="services">The ServiceCollection instance</param>
  private static void AddErrorHandlers(IServiceCollection services) {
    // See: https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-9.0
    services.AddProblemDetails(/*o => { o.CustomizeProblemDetails = context => { context.Exception.Dump(); }; }*/);

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

    services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly(), ServiceLifetime.Transient);
  }
}

/*public class CustomResultFactory : IFluentValidationAutoValidationResultFactory {
  public IActionResult CreateActionResult(ActionExecutingContext context,
    ValidationProblemDetails? validationProblemDetails) {
    return new BadRequestObjectResult(new
      { Title = "Validation errors" });
  }
}*/