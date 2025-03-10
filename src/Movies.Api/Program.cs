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

app.UseHttpsRedirection();

/*app.MapGet("/test", async (DatabaseContext dbContext) => {
  /*var user = await dbContext.Users.Where(x => x.Id == 28)
    .Select(x => new {
      x.Email
    }).FirstOrDefaultAsync();#1#

  var user = await dbContext.Users.Where(x => x.Id == 282)
    .Select(x => x.Email).FirstOrDefaultAsync();
  return new {
    user
  };
});*/

app.Run();