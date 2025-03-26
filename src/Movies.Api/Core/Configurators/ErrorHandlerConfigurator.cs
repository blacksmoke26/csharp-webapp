// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See: https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-9.0

using System.Reflection;
using Movies.Api.Core.Interfaces;
using Movies.Application.Config;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Movies.Api.Core.Configurators;

public abstract class ErrorHandlerConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the rate-limiter to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration _) {
    services.AddProblemDetails( /*o => { o.CustomizeProblemDetails = context => { context.Exception.Dump(); }; }*/);

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

    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);
  }

  /// <summary>
  /// Configures the error handles to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.UseExceptionHandler();
    app.UseStatusCodePages();
  }
}

/*public class CustomResultFactory : IFluentValidationAutoValidationResultFactory {
  public IActionResult CreateActionResult(ActionExecutingContext context,
    ValidationProblemDetails? validationProblemDetails) {
    return new BadRequestObjectResult(new
      { Title = "Validation errors" });
  }
}*/