// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Website: https://github.com/blacksmoke26/

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Movies.Application.Domain.Metadata;

public record UserMetadata {
  [StringLength(10), JsonPropertyName("language")]
  public string Language { get; set; } = "en-US";
  [StringLength(20), JsonPropertyName("timezone")]
  public string Timezone { get; set; } = "UTC";
  public UserMetadataSecurity Security { get; set; } = new();
  public UserMetadataActivation Activation { get; set; } = new();
  public UserMetadataPassword Password { get; set; } = new();
  public UserMetadataLoggedInHistory LoggedInHistory { get; set; } = new();
}

[Owned]
public record UserMetadataActivation {
  public string? Code { get; set; }
  public bool? Pending { get; set; }
  public DateTime? RequestedAt { get; set; }
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
  [MaxLength(20)]
  public string? LastIp { get; set; } = null;
  public DateTime? LastDate { get; set; }
  public int SuccessCount { get; set; } = 0;
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
  public DateTime? LastResetAt { get; set; }
  [StringLength(IdentityHelper.PasswordResetCodeSize)]
  public string? ResetCode { get; set; }
  public DateTime? ResetCodeRequestedAt { get; set; }
  public int ResetCount { get; set; }
  public DateTime? LastUpdatedAt { get; set; }
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
  public bool TokenInvalidate { get; set; }
}