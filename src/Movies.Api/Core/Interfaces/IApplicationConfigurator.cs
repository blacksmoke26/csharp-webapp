// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Api.Core.Interfaces;

public interface IApplicationConfigurator {
  /// <param name="app">The WebApplication instance</param>
  public static abstract void Use(WebApplication app);
}