// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models;

public class Rating : ModelBase {
  [Key] [Comment("Primary Key")]
  public long Id { get; set; }

  [Required] [Comment("Owner User")]
  public long UserId { get; set; }

  [Required] [Comment("Target Movie")]
  public long MovieId { get; set; }

  [Required] [Comment("Score")]
  public short Score { get; set; }

  [MaxLength(1000)] [Comment("User's Feedback")]
  public string? Feedback { get; set; }

  [Comment("Created Date")]
  public DateTime? CreatedAt { get; set; }

  [Comment("Last Updated")]
  public DateTime? UpdatedAt { get; set; }

  /*/// <summary>
  /// The movie associated with this rating
  /// </summary>
  public virtual Movie Movie { get; set; } = null!;

  /// <summary>
  /// The user associated with this rating
  /// </summary>
  public virtual User User { get; set; } = null!;*/

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(
    EntityState state, CancellationToken cancellationToken = default) {
    if (state is EntityState.Added)
      CreatedAt = DateTime.UtcNow;

    if (state is EntityState.Added or EntityState.Modified)
      UpdatedAt = DateTime.UtcNow;

    return base.OnTrackChangesAsync(state, cancellationToken);
  }
}