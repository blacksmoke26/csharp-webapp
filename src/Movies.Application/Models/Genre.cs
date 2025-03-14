// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;

namespace Movies.Application.Models;

public class Genre : ModelBase {
  [Key] [Comment("Primary Key")]
  public long Id { get; set; }

  [Required] [Comment("Owner Movie")]
  public long MovieId { get; set; }

  [Required] [MaxLength(60)] [Comment("Name")]
  public string Name { get; set; } = null!;
}