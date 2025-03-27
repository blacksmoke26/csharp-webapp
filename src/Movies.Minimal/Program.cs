// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
// Guide: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata
// Scalar: https://github.com/scalar/scalar/blob/main/documentation/integrations/dotnet.md

using Movies.Minimal.Core.Extensions;
using Movies.Minimal.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InitBootstrapper(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(x => {
  //x.ShouldInclude = description => description.GroupName == null || description.GroupName == x.DocumentName;
});

var app = builder.Build();

app.UseBootstrapper();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.MapOpenApi().AllowAnonymous();
  app.MapScalarApiReference();
  //app.UseHttpsRedirection();
}

app.MapApiEndpoints();

app.Run();