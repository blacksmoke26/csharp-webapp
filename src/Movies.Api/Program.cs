// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Movies.Api.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitBootstrapper(builder.Configuration);

var app = builder.Build();

app.UseBootstrapper();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  //app.UseDeveloperExceptionPage();
}

if (!app.Environment.IsDevelopment()) {
  app.UseHttpsRedirection();
}

app.Run();