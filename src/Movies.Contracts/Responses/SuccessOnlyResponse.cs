// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

namespace Movies.Contracts.Responses;

/// <summary>
/// This class represents the success response without containing
/// any kind of data, the best usage is when you delete/remove
/// something and returns as an empty success response 
/// </summary>
public class SuccessOnlyResponse : ISuccessResponse {
  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public bool Success => true;
}