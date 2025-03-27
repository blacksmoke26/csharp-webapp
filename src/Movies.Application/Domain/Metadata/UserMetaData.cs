// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Movies.Application.Domain.Metadata;

public record UserMetadata {
  [StringLength(10), JsonPropertyName("language")]
  public string Language { get; set; } = "en-US";

  [StringLength(20), JsonPropertyName("timezone")]
  public string Timezone { get; set; } = "UTC";

  [JsonPropertyName("security")]
  public UserMetadataSecurity Security { get; set; } = new();

  [JsonPropertyName("activation")]
  public UserMetadataActivation Activation { get; set; } = new();

  [JsonPropertyName("password")]
  public UserMetadataPassword Password { get; set; } = new();

  [JsonPropertyName("loggedInHistory")]
  public UserMetadataLoggedInHistory LoggedInHistory { get; set; } = new();
}

[Owned]
public record UserMetadataActivation {
  [JsonPropertyName("code")]
  public string? Code { get; set; }

  [JsonPropertyName("pending")]
  public bool? Pending { get; set; }

  [JsonPropertyName("requestedAt")]
  public DateTime? RequestedAt { get; set; }

  [JsonPropertyName("completedAt")]
  public DateTime? CompletedAt { get; set; }

  /// <summary>An event method which invoked which account successfully created</summary>
  public void OnSignedUp() {
    Pending = true;
    Code = IdentityHelper.GenerateActivationCode();
    RequestedAt = DateTime.UtcNow;
  }

  /// <summary>An event method which invoked which account successfully verified</summary>
  public void OnActivated() {
    Pending = false;
    Code = null;
    CompletedAt = DateTime.UtcNow;
  }
}

[Owned]
public record UserMetadataLoggedInHistory {
  [MaxLength(20)] [JsonPropertyName("lastIp")]
  public string? LastIp { get; set; } = null;

  [JsonPropertyName("lastDate")]
  public DateTime? LastDate { get; set; }

  [JsonPropertyName("successCount")]
  public int SuccessCount { get; set; } = 0;

  [JsonPropertyName("failedCount")]
  public int FailedCount { get; set; } = 0;

  /// <summary>An event method which invoked upon successful authentication</summary>
  /// <param name="ipAddress">The current IP address</param>
  public void OnLogin(string? ipAddress = null) {
    LastIp = ipAddress;
    LastDate = DateTime.UtcNow;
    FailedCount = 0;
    SuccessCount += 1;
  }
}

[Owned]
public record UserMetadataPassword {
  [JsonPropertyName("lastResetAt")]
  public DateTime? LastResetAt { get; set; }

  [StringLength(IdentityHelper.PasswordResetCodeSize), JsonPropertyName("resetCode")]
  public string? ResetCode { get; set; }

  [JsonPropertyName("resetCodeRequestedAt")]
  public DateTime? ResetCodeRequestedAt { get; set; }

  [JsonPropertyName("resetCount")]
  public int ResetCount { get; set; }

  [JsonPropertyName("lastUpdatedAt")]
  public DateTime? LastUpdatedAt { get; set; }

  [JsonPropertyName("updatedCount")]
  public int UpdatedCount { get; set; }

  /// <summary>An event method which invoked upon password changing</summary>
  public void OnUpdate() {
    UpdatedCount += 1;
    LastUpdatedAt = DateTime.UtcNow;
  }

  /// <summary>An event method which invoked upon requesting for a password reset</summary>
  public void OnResetRequest() {
    ResetCode = IdentityHelper.GeneratePasswordResetCode();
    ResetCodeRequestedAt = DateTime.UtcNow;
  }

  /// <summary>An event method which invoked upon resetting the password</summary>
  public void OnReset() {
    ResetCode = null;
    ResetCodeRequestedAt = null;
    ResetCount += 1;
    LastResetAt = DateTime.UtcNow;
  }

  /// <summary>Verifies the reset code is expired or not</summary>
  public bool IsResetCodeExpired() {
    if (ResetCodeRequestedAt is null) return false;
    return ResetCodeRequestedAt.Value.ToUniversalTime().AddDays(3d)
      .CompareTo(DateTime.UtcNow) < 0;
  }
}

[Owned]
public record UserMetadataSecurity {
  [JsonPropertyName("tokenInvalidate")]
  public bool TokenInvalidate { get; set; }
}