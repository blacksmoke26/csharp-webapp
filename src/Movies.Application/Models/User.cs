// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

namespace Movies.Application.Models;

public enum UserRole {
  User = 0,
  Admin = 1
}

public class User {
  public int Id { get; set; }
  
  public UserRole Role { get; set; } = UserRole.User;
  
  public required string Name { get; set; }
  
  public required string Email { get; set; }
  
  public string Password { get; set; } = string.Empty;
}