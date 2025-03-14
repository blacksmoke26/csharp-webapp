using Microsoft.Extensions.Logging;

namespace Movies.Application.Config;

/// <summary>
/// DatabaseConfiguration presents the options required by the DatabaseContext class
/// <p>Check <see cref="AppConfiguration"/> class for usage.</p>
/// </summary>
public struct DatabaseConfiguration {
  public required string ConnectionString { get; init; }
  public required DatabaseLoggingConfiguration Logging { get; init; }
}

/// <summary>
/// DatabaseLoggingConfiguration presents the options required by the `DatabaseConfiguration` class
/// <p>Check <see cref="DatabaseConfiguration"/> class for usage.</p>
/// </summary>
public struct DatabaseLoggingConfiguration {
  public bool Enabled { get; init; }
  public LogLevel LogLevel { get; init; }
}