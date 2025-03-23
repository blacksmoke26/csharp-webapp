// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Application.Config;

public struct RateLimiterConfiguration {
  public int WindowSeconds { get; set; }
  public int PermitLimit { get; set; }
  public IDictionary<string, RateLimiterPolicyConfiguration> Policies { get; set; }
}

public struct RateLimiterPolicyConfiguration {
  public int PermitLimit { get; set; }
}