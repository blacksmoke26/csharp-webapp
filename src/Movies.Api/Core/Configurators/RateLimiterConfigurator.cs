// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp
// Guide: https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-9.0
// Read more: https://abp.io/community/articles/rate-limiting-with-asp.net-core-7.0-co55vem3
// Read more: https://medium.com/@solomongetachew112/implementing-rate-limiting-in-asp-net-core-web-api-net-8-da7f82442fe0

using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Movies.Api.Core.Interfaces;
using Movies.Application.Config;

namespace Movies.Api.Core.Configurators;

public static class RateLimiterPolicies {
  public const string Authenticated = "Authenticated";
  public const string Uncapped = "Uncapped";
}

public abstract class RateLimiterConfigurator : IApplicationServiceConfigurator {
  /// <summary>
  /// Configures the rate-limiter to the service collection
  /// </summary>
  /// <inheritdoc/>
  public static void Configure(IServiceCollection services, AppConfiguration appConfig) {
    var config = appConfig.GetConfig().GetRequiredSection("RateLimiter").Get<RateLimiterConfiguration>();

    services.AddRateLimiter(options => {
      options.GlobalLimiter = OptionsGlobalLimiter(config);

      options.OnRejected = OptionsOnRejected();

      options.AddPolicy(RateLimiterPolicies.Uncapped, UncappedPolicyPartitioner(config));
    });
  }

  /// <summary>
  /// Configures the rate-limiter to the web application
  /// </summary>
  /// <inheritdoc/>
  public static void Use(WebApplication app) {
    app.UseRateLimiter();
  }

  /// <summary>
  /// <see cref="Microsoft.AspNetCore.RateLimiting.RateLimitingMiddleware"/> that handles GlobalLimiter requests by this middleware.
  /// </summary>
  private static PartitionedRateLimiter<HttpContext> OptionsGlobalLimiter(RateLimiterConfiguration config) {
    return PartitionedRateLimiter.Create<HttpContext, string>(httpContext => {
      var authConfig = config.Policies[RateLimiterPolicies.Authenticated];
      var permitLimit = httpContext.GetAuthKey() is not null ? authConfig.PermitLimit : config.PermitLimit;

      return RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.GetAuthKey() ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        factory: _ => new FixedWindowRateLimiterOptions {
          Window = TimeSpan.FromSeconds(config.WindowSeconds),
          PermitLimit = permitLimit,
        });
    });
  }

  /// <summary>
  /// A custom policy which enables the `uncapped` request limiter.
  /// </summary>
  private static Func<HttpContext, RateLimitPartition<string>> UncappedPolicyPartitioner(
    RateLimiterConfiguration limiterConfig) {
    var uncappedConfig = limiterConfig.Policies[RateLimiterPolicies.Uncapped];

    return _ => {
      return RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: RateLimiterPolicies.Uncapped,
        factory: _ => new FixedWindowRateLimiterOptions {
          Window = TimeSpan.FromSeconds(limiterConfig.WindowSeconds),
          PermitLimit = uncappedConfig.PermitLimit,
        });
    };
  }

  /// <summary>
  /// <see cref="Microsoft.AspNetCore.RateLimiting.OnRejectedContext" /> that handles requests rejected by this middleware.
  /// </summary>
  private static Func<OnRejectedContext, CancellationToken, ValueTask> OptionsOnRejected() {
    return (context, token) => {
      if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)) {
        context.HttpContext.Response.Headers.RetryAfter =
          ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
      }

      context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
      context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
        .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
        .LogWarning("[429] TooManyRequests: {RequestPath}", context.HttpContext.Request.Path);

      context.HttpContext.Response.WriteAsJsonAsync(new OperationFailureResponse() {
        ErrorCode = "REQUEST_BLOCKED",
        Errors = [
          new() {
            Message = "Too many requests. Please try later again..."
          }
        ]
      }, token);

      return new ValueTask();
    };
  }
}