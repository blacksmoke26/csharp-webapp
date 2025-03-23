// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See also: https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties
// See also: https://www.npgsql.org/efcore/modeling/generated-properties.html?tabs=13%2Cefcore5
// See also: https://medium.com/@serhiikokhan/jsonb-in-postgresql-with-ef-core-cc945f1aba2a
// See also: https://mbarkt3sto.hashnode.dev/ef-core-and-postgresql-working-with-json-data
// See also: https://www.npgsql.org/efcore/mapping/translations.html

using System.Globalization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage;

namespace Movies.Application.Database;

public class DatabaseContext : DbContext {
  /// <summary>The database configuration</summary>
  private readonly DatabaseConfiguration _config;

  /// <summary>Users entity object</summary>
  public DbSet<User> Users { get; set; }

  /// <summary>Movies entity object</summary>
  public DbSet<Movie> Movies { get; set; }

  /// <summary>Ratings entity object</summary>
  public DbSet<Rating> Ratings { get; set; }

  /// <summary>Genres entity object</summary>
  public DbSet<Genre> Genres { get; set; }

  /// <inheritdoc cref="Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.BeginTransactionAsync"/>
  public DatabaseContext(DatabaseConfiguration config) {
    _config = config;
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
  }

  /// <inheritdoc cref="Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade.BeginTransactionAsync"/>
  public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default) {
    return Database.BeginTransactionAsync(token);
  }

  /// <summary>Prints the query result to the console</summary>
  /// <param name="message">Message details</param>
  private void LogToConsole(string message) {
    if (_config.Logging.Enabled) Console.WriteLine(message);
  }

  /// <inheritdoc/>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder
      .LogTo(LogToConsole, _config.Logging.LogLevel)
      .UseNpgsql(_config.ConnectionString, x =>
        x.ConfigureDataSource(y => {
          // Parse JSON as dynamic Object
          // See: https://www.npgsql.org/doc/types/json.html?tabs=datasource
          y.EnableDynamicJson();
          y.ConfigureJsonOptions(new JsonSerializerOptions(JsonSerializerDefaults.Web));
          y.EnableParameterLogging();
        }))
      // use snake_care for table and columns name explicitly 
      .UseSnakeCaseNamingConvention(CultureInfo.InvariantCulture)
      .EnableDetailedErrors();
  }

  /// <inheritdoc/>
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.UseIdentityByDefaultColumns();
    modelBuilder.Entity<User>(entity => {
      entity.Property(b => b.Metadata)
        .HasColumnType("jsonb"); // Use jsonb data type for PostgreSQL
    });
  }

  /// <inheritdoc/>
  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new()) {
    var modifiedEntities = ChangeTracker.Entries();

    foreach (var entity in modifiedEntities) {
      if (entity.Entity is ModelBase model)
        await model.OnTrackChangesAsync(entity.State, cancellationToken);
    }

    return await base.SaveChangesAsync(cancellationToken);
  }
}