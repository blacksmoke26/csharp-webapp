// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Movies.Api;
using Movies.Api.Mapping;
using Movies.Api.Services;
using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddSingleton<AppConfig>();

#region JWT Authentication / Authorization

builder.Services.AddAuthentication(x => {
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
  x.TokenValidationParameters = new TokenValidationParameters() {
    IssuerSigningKey = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(config["Jwt:Key"]!)
    ),
    ValidateIssuerSigningKey = true,
    ValidateLifetime = true,
    ValidIssuer = config["Jwt:Issuer"],
    ValidAudience = config["Jwt:Audience"],
    ValidateIssuer = true,
    ValidateAudience = true
  };
});

builder.Services.AddAuthorization(x => {
  // Policy Type: Admin
  x.AddPolicy(AuthPolicies.AdminPolicy, p => { p.RequireRole(UserRoles.Admin); });

  // Policy Type: User
  x.AddPolicy(AuthPolicies.UserPolicy, p => { p.RequireRole(UserRoles.User); });

  // Policy Type: Trusted
  x.AddPolicy(AuthPolicies.TrustedPolicy, p => {
    p.RequireRole(UserRoles.Admin);
    p.RequireRole(UserRoles.User);
  });
});

builder.Services.AddTransient<AuthService>();

#endregion

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
}

app.UseHttpsRedirection();

#region JWT Authentication / Authorization

app.UseAuthentication();
app.UseAuthorization();

#endregion

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

/*app.MapGet("/test", () => {
  var result = new {
    Success = true,
  };
  return result;
});*/

app.Run();