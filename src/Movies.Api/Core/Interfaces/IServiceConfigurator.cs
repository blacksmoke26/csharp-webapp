﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Config;

namespace Movies.Api.Core.Interfaces;

public interface IServiceConfigurator {
  /// <param name="services">The ServiceCollection instance</param>
  /// <param name="config">The AppConfiguration instance</param>
  public static abstract void Configure(IServiceCollection services, AppConfiguration config);
}