﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models;

[Table("ratings")]
[Index("UserId", "MovieId", Name = "IDX_ratings_user_id_movie_id", IsUnique = true)]
public class Rating : ModelBase {
  /// <summary>ID</summary>
  [Key] [Column("id")]
  public long Id { get; set; }

  /// <summary>User</summary>
  [Column("user_id")]
  public long UserId { get; set; }

  /// <summary>Movie</summary>
  [Column("movie_id")]
  public long MovieId { get; set; }

  /// <summary>Score</summary>
  [Column("score")]
  public short Score { get; set; }

  /// <summary>Feedback</summary>
  [MaxLength(1000), Column("feedback")]
  public string? Feedback { get; set; }

  /// <summary>Created</summary>
  [Column("created_at", TypeName = "timestamp without time zone")]
  public DateTime? CreatedAt { get; set; }

  /// <summary>Updated</summary>
  [Column("updated_at", TypeName = "timestamp without time zone")]
  public DateTime? UpdatedAt { get; set; }

  [ForeignKey("MovieId")] [InverseProperty("Ratings")]
  public virtual Movie Movie { get; set; } = null!;

  [ForeignKey("UserId")] [InverseProperty("Ratings")]
  public virtual User User { get; set; } = null!;

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(EntityState state, CancellationToken token = default) {
    if (state is EntityState.Added)
      CreatedAt = DateTime.UtcNow;

    if (state is EntityState.Added or EntityState.Modified)
      UpdatedAt = DateTime.UtcNow;

    return Task.CompletedTask;
  }
}