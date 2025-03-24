// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using Microsoft.Extensions.DependencyInjection;
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
  public static void AddApplication(this IServiceCollection services) {
    // repos
    services.AddSingleton<GenreRepository>();
    services.AddSingleton<MovieRepository>();
    services.AddSingleton<RatingRepository>();
    services.AddSingleton<UserRepository>();

    // services
    services.AddSingleton<IdentityService>();
    services.AddSingleton<GenreService>();
    services.AddSingleton<MovieService>();
    services.AddSingleton<RatingService>();
    services.AddSingleton<UserService>();

    services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
  }

  /// <summary>
  /// Register the database-specific services
  /// </summary>
  /// <param name="services">ServiceCollection instance</param>
  /// <param name="config">Application configuration</param>
  /// <returns>The updated service collection instance</returns>
  public static void AddDatabase(this IServiceCollection services, AppConfiguration config) {
    services.AddSingleton<MovieDbContext>(_ => {
      MovieDbContext instance = new(config.DbConfig());
      instance.Database.ExecuteSql($"SET timezone = '+00:00'");
      return instance;
    });
  }
}