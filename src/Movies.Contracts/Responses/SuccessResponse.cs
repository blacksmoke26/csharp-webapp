// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp


namespace Movies.Contracts.Responses;

/// <summary>
/// This response class formats the successful success response using dynamic data.
/// </summary>
/// <param name="data">The dynamic object</param>
public class SuccessResponse (object data) : ISuccessResponse {
  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public bool Success => true;

  /// <summary>
  /// This response class  represents the successful response
  /// containing the `data` property
  /// </summary>
  public object Data { get; set; } = data;
}