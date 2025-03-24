﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models;

[Table("movies")]
[Index("Title", Name = "IDX_movies_title")]
[Index("Title", "YearOfRelease", Name = "UNQ_Title_YoR", IsUnique = true)]
[Index("Slug", Name = "UNQ_slug", IsUnique = true)]
public class Movie : ModelBase {
  /// <summary>ID</summary>
  [Key] [Column("id")]
  public long Id { get; set; }

  /// <summary>User</summary>
  [Column("user_id")]
  public long UserId { get; set; }

  /// <summary>Title</summary>
  [Column("title")] [StringLength(60)]
  public string Title { get; set; } = null!;

  /// <summary>Year of Release</summary>
  [Column("year_of_release")]
  public short YearOfRelease { get; set; }

  /// <summary>Slug</summary>
  [Column("slug")] [StringLength(90)]
  public string Slug { get; set; } = null!;

  /// <summary>Status</summary>
  [Column("status")]
  public MovieStatus Status { get; set; } = MovieStatus.Pending;

  /// <summary>Created</summary>
  [Column("created_at", TypeName = "timestamp without time zone")]
  public DateTime? CreatedAt { get; set; }

  /// <summary>Updated</summary>
  [Column("updated_at", TypeName = "timestamp without time zone")]
  public DateTime? UpdatedAt { get; set; }

  [InverseProperty("Movie")]
  public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

  [InverseProperty("Movie")]
  public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

  [ForeignKey("UserId")] [InverseProperty("Movies")]
  public virtual User User { get; set; } = null!;

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(EntityState state, CancellationToken token = default) {
    if (state is EntityState.Added) {
      CreatedAt = DateTime.UtcNow;
    }

    if (state is EntityState.Added or EntityState.Modified) {
      this.GenerateSlug();
      UpdatedAt = DateTime.UtcNow;
    }

    return Task.CompletedTask;
  }
}