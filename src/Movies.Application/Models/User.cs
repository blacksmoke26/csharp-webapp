// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movies.Contracts.Responses.Identity;

namespace Movies.Application.Models;

public class User : ModelBase {
  [Key] [Comment("Primary Key")]
  public long Id { get; set; }

  [MaxLength(20)] [Comment("First name")]
  public string FirstName { get; set; } = null!;

  [MaxLength(20)] [Comment("Last name")]
  public string LastName { get; set; } = null!;

  [MaxLength(255)] [Comment("Email Address")]
  public string Email { get; set; } = null!;

  [MaxLength(60)] [Comment("Password")]
  public string Password { get; set; } = null!;

  [MaxLength(64)] [Comment("Authorization Key")]
  public string AuthKey { get; set; } = null!;

  [MaxLength(150)] [Comment("Password Hash")]
  public string PasswordHash { get; set; } = null!;

  [MaxLength(100)] [Comment("Password Reset Token")]
  public string? PasswordResetToken { get; set; }

  [Required] [MaxLength(20)] [Comment("Role")]
  public string Role { get; set; } = null!;

  [Comment("Account Status")]
  public UserStatus Status { get; set; }

  [Comment("Additional Metadata")] [Column(TypeName = "jsonb")]
  public UserMetadata Metadata { get; set; } = null!;

  [Comment("Created Date")]
  public DateTime CreatedAt { get; set; }

  [Comment("Last Updated")]
  public DateTime? UpdatedAt { get; set; }

  /// <summary>
  /// List of movies associated with this user
  /// </summary>
  public virtual List<Movie> Movies { get; set; } = [];

  /// <summary>
  /// List of ratings associated with this user
  /// </summary>
  public virtual List<Rating> Ratings { get; set; } = [];

  /// <summary>
  /// Returns the user's fullname
  /// </summary>
  [NotMapped]
  public virtual string FullName => string.Concat(FirstName.Trim(), " ", LastName.Trim());

  /// <summary>
  /// Encrypt the password and also generate the password hash
  /// </summary>
  /// <param name="password">The password to set</param>
  public void SetPassword(string password) {
    var result = IdentityHelper.EncryptPassword(password);
    Password = result.Password;
    PasswordHash = result.PasswordHash;
  }

  /// <summary>
  /// Sets the token invalidation state
  /// </summary>
  /// <param name="state">The invalidate state to set</param>
  public void SetTokenInvalidateState(bool state) => Metadata.Security.TokenInvalidate = state;

  /// <summary>
  /// Validates the given password against the existing password
  /// </summary>
  /// <param name="password">The password to verify</param>
  /// <returns>True if password is correct, false otherwise</returns>
  public bool ValidatePassword(string password) =>
    IdentityHelper.ValidatePassword(new EncryptedPasswordResult {
      Password = password,
      PasswordHash = PasswordHash,
    });

  /// <summary>
  /// Generates the password reset token
  /// </summary>
  public void GeneratePasswordResetToken()
    => PasswordResetToken = IdentityHelper.GeneratePasswordResetToken();

  /// <summary>
  /// Removes password reset token
  /// </summary>
  public void RemovePasswordResetToken() => PasswordResetToken = null;

  /// <summary>
  /// Generates token authentication key
  /// </summary>
  public void GenerateAuthKey()
    => AuthKey = IdentityHelper.GenerateAuthKey();

  /// <summary>
  /// Validates the given auth key
  /// </summary>
  /// <param name="authKey">The key to validate</param>
  /// <returns>True when the key is valid, False otherwise</returns>
  public bool ValidateAuthKey(string authKey) => AuthKey == authKey;

  /// <inheritdoc/>
  public override Task OnTrackChangesAsync(
    EntityState state, CancellationToken cancellationToken = default) {
    if (state is EntityState.Added) {
      GenerateAuthKey();
      CreatedAt = DateTime.UtcNow;
    }
    else if (state is EntityState.Added or EntityState.Modified) {
      UpdatedAt = DateTime.UtcNow;
    }

    return base.OnTrackChangesAsync(state, cancellationToken);
  }

  /// <summary>
  /// Map the current user to the <c>UserLoggedInDetails</c> object
  /// </summary>
  /// <returns>The mapped object</returns>
  public UserAuthInfo ToLoggedInDetails() {
    return new UserAuthInfo {
      Fullname = FullName,
      FirstName = FirstName,
      LastName = LastName,
      Email = Email,
      Role = Role
    };
  }

  /// <summary>
  /// Map the current user to the <c>UserMeDetails</c> object
  /// </summary>
  /// <returns>The mapped object</returns>
  public UserMeResponse ToMeDetails() {
    return new UserMeResponse {
      Id = Id,
      Fullname = FullName,
      FirstName = FirstName,
      LastName = LastName,
      Email = Email,
      Role = Role,
      Status = Status.ToString().ToLower(),
      CreatedAt = CreatedAt,
    };
  }
}

public class UserMetadataActivation {
  public bool? Pending { get; set; }
  public DateTime? RequestedAt { get; set; }
  public DateTime? CompletedAt { get; set; }
}

public class UserMetadataLoggedInHistory {
  public string? LastIp { get; set; }
  public DateTime? LastDate { get; set; }
  public int SuccessCount { get; set; } = 0;
  public int FailedCount { get; set; } = 0;

  /// <summary>An event method which invoked which user successfully authenticated</summary>
  /// <summary>An event method which invoked upon authentication</summary>
  /// <param name="ipAddress">The current IP address</param>
  public void OnLogin(string? ipAddress = null) {
    LastIp = ipAddress;
    LastDate = DateTime.UtcNow;
    FailedCount = 0;
    SuccessCount += 1;
  }
}

public class UserMetadataPassword {
  public DateTime? LastResetAt { get; set; }
  public string? ResetCode { get; set; }
  public DateTime? ResetCodeRequestedAt { get; set; }
  public int ResetCount { get; set; } = 0;
  public DateTime? LastUpdatedAt { get; set; }
  public int UpdatedCount { get; set; } = 0;

  /// <summary>An event method which invoked upon changing the password</summary>
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

  public bool IsResetCodeExpired() {
    if (ResetCodeRequestedAt is null) return false;
    return ResetCodeRequestedAt.Value.ToUniversalTime().AddDays(3d)
      .CompareTo(DateTime.UtcNow) < 0;
  }
}

public class UserMetadataSecurity {
  public bool? TokenInvalidate { get; set; }
}

[Keyless]
[ComplexType]
public class UserMetadata {
  public string Language { get; set; } = "en-US";
  public string Timezone { get; set; } = "UTC";
  public UserMetadataSecurity Security { get; set; } = new();
  public UserMetadataActivation Activation { get; set; } = new();
  public UserMetadataPassword Password { get; set; } = new();
  public UserMetadataLoggedInHistory LoggedInHistory { get; set; } = new();
}