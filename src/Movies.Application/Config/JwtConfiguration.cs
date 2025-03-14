namespace Movies.Application.Config;

/// <summary>
/// JwtConfiguration presents the options required by JWT Auth service
/// <p>Check <see cref="AppConfiguration"/> class for usage.</p>
/// </summary>
public struct JwtConfiguration {
  /// <summary>Represents a symmetric security key.</summary>
  public string? Key { get; init; }

  /// <summary>identifies the principal that issued the JWT</summary>
  public string Issuer { get; init; }

  /// <summary>Identifies the recipients that the JWT is intended for.</summary>
  public string Audience { get; init; }

  /// <summary>The expiration time in hours  on or after which the JWT MUST NOT be accepted for processing</summary>
  public int ExpirationInHours { get; init; }
}