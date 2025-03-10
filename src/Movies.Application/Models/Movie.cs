// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See more: https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one

using System.ComponentModel.DataAnnotations;
using CaseConverter;
using Microsoft.EntityFrameworkCore;
using Movies.Application.Core.Bases;
using Movies.Application.Repositories;

namespace Movies.Application.Models;

public class Movie : ModelBase {
  [Key] [Comment("Primary Key")]
  public long Id { get; set; }

  [Required] [Comment("Owner User")]
  public long UserId { get; set; }

  [Required] [MaxLength(60)] [Comment("Movie name")]
  public string Title { get; set; } = null!;

  [Required] [Comment("Year of Release")]
  public short YearOfRelease { get; set; }

  [MaxLength(255)] [Comment("Slug")]
  public string? Slug { get; set; }

  [Comment("Status")]
  public MovieStatus Status { get; set; } = MovieStatus.Published;

  [Comment("Created Date")]
  public DateTime? CreatedAt { get; set; }

  [Comment("Last Updated")]
  public DateTime? UpdatedAt { get; set; }

  /// <summary>
  /// List of genres associated with this movie
  /// </summary>
  public virtual List<Genre> Genres { get; set; } = [];

  /// <summary>
  /// List of ratings associated with this movie
  /// </summary>
  public virtual List<Rating> Ratings { get; set; } = [];

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(
    EntityState state, CancellationToken cancellationToken = default) {
    if (state is EntityState.Added)
      CreatedAt = DateTime.UtcNow;
    else if (state is EntityState.Added or EntityState.Modified) {
      GenerateSlug();
      UpdatedAt = DateTime.UtcNow;
    }

    return base.OnTrackChangesAsync(state, cancellationToken);
  }

  /// <summary>
  /// Generates a slug based on a movie name and the year of release
  /// </summary>
  public void GenerateSlug()
    => Slug = string.Concat(Title, " ", YearOfRelease).ToKebabCase();
}