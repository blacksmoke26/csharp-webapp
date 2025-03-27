// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository: https://github.com/blacksmoke26/csharp-webapp
// See also https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/examples
// See also https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments

namespace Movies.Application.Config;

/// <summary>
/// AuthenticationConfiguration presents the options required by the `Authentication` process
/// <p>Check <see cref="DatabaseConfiguration"/> class for usage.</p>
/// </summary>
public struct AuthenticationConfiguration {
  /// <summary>
  /// If enabled, whenever the user logouts, the <i>authentication key</i> will be automatically rotated.
  /// <remarks>This will invalidate the token completely and user have to sign-in again.</remarks>
  /// </summary>
  public bool ExpireTokenAfterLogout { get; init; }
}
