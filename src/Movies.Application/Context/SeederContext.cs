// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Application.Seeders;

namespace Movies.Application.Context;

public class SeederContext(DbContext context) {
  public async Task InitializeAsync(CancellationToken token) {
    Console.WriteLine("Initializing seeders...");

    await UserSeeder.InitializeAsync(context, token);
  }
}