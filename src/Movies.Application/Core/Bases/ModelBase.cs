﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// See also: [Entity Framework Core Generator](https://efg.loresoft.com/)

namespace Movies.Application.Core.Bases;

/// <summary>
/// ModelBase represents the parent class of all model classes
/// </summary>
public abstract class ModelBase {
  /// <summary>
  /// A callback function which triggers while db.SaveChanges() calls
  /// </summary>
  /// <param name="state">The changes state</param>
  /// <param name="token">The cancellation token</param>
  /// <seealso cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/>
  /// <seealso href="https://mbarkt3sto.hashnode.dev/how-to-auto-update-created-updated-and-deleted-timestamps-in-entity-framework-core">
  /// How to Auto Update Created, Updated and Deleted Timestamps in Entity Framework Core?</seealso>
  /// <returns>The results</returns>
  public virtual Task OnTrackChangesAsync(
    EntityState state, CancellationToken token = default) {
    return Task.CompletedTask;
  }
}