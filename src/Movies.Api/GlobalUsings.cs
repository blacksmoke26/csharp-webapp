// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

// Global using directives

global using System.Text;
global using Asp.Versioning;
global using FluentValidation;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Movies.Api.Core.Constants;
global using Movies.Api.Core.Extensions;
global using Movies.Api.Domain.Query.Validators;
global using Movies.Application;
global using Movies.Application.Core.Extensions;
global using Movies.Application.Domain.Filters;
global using Movies.Application.Helpers;
global using Movies.Application.Objects;
global using Movies.Application.Repositories;
global using Movies.Application.Services;
global using Movies.Contracts.Requests.Payload;
global using Movies.Contracts.Requests.Query;
global using Movies.Contracts.Responses;
global using Swashbuckle.AspNetCore.Annotations;