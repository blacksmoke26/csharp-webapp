// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;
using Movies.Application.Services;

namespace Movies.Application;

/// <summary>
/// Represents as the part of dependency injection to register the dependencies inside the application
/// </summary>
public static class ApplicationServiceCollectionExtensions {
  /// <summary>
  /// Registers application level services 
  /// </summary>
  /// <param name="services">ServiceCollection instance</param>
  /// <returns>The updated service collection instance</returns>
  public static IServiceCollection AddApplication(this IServiceCollection services) {
    // repos
    services.AddSingleton<MovieRepository>();
    services.AddSingleton<GenreRepository>();
    services.AddSingleton<RatingRepository>();
    services.AddSingleton<UserRepository>();

    // services
    services.AddSingleton<IdentityService>();
    services.AddSingleton<MovieService>();
    services.AddSingleton<UserService>();

    services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
    
    return services;
  }

  /// <summary>
  /// Register the database-specific services
  /// </summary>
  /// <param name="services">ServiceCollection instance</param>
  /// <param name="connectionString">Database connection string to connect</param>
  /// <returns>The updated service collection instance</returns>
  public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString) {
    services.AddSingleton<DatabaseContext>(_ => {
      DatabaseContext instance = new(connectionString);
      instance.Database.ExecuteSql($"SET timezone = '+00:00'");
      return instance;
    });
    return services;
  }
}