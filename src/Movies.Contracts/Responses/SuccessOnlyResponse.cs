﻿// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

public record SuccessOnlyResponse : ISuccessResponse {
  [JsonPropertyName("success"), Description("The operation was successful")]
  public bool Success => true;
}