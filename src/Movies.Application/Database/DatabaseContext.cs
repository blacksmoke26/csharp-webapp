// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See also: https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties
// See also: https://www.npgsql.org/efcore/modeling/generated-properties.html?tabs=13%2Cefcore5
// See also: https://medium.com/@serhiikokhan/jsonb-in-postgresql-with-ef-core-cc945f1aba2a

using Microsoft.EntityFrameworkCore;
using Movies.Application.Core.Bases;
using Movies.Application.Models;

namespace Movies.Application.Database;

public class DatabaseContext : DbContext {
  private readonly string _connectionString;

  public DatabaseContext(string connectionString) {
    _connectionString = connectionString;
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
  }

  public DbSet<User> Users { get; set; }
  public DbSet<Movie> Movies { get; set; }
  public DbSet<Rating> Ratings { get; set; }
  public DbSet<Genre> Genres { get; set; }

  /// <inheritdoc/>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    optionsBuilder
      .UseNpgsql(_connectionString, x =>
        x.ConfigureDataSource(x
          // Parse JSON as dynamic Object
          => x.EnableDynamicJson()
        ))
      // use snake_care for table and columns name explicitly 
      .UseSnakeCaseNamingConvention();
  }

  /// <inheritdoc/>
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.UseIdentityByDefaultColumns();
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