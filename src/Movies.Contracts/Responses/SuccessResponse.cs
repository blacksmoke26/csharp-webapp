// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Movies.Contracts.Responses;

public class SuccessResponse : ISuccessResponse {
  /// <summary>
  /// The success property means operation was a success 
  /// </summary>
  public bool Success => true;

  /// <summary>
  /// This response class  represents the successful response
  /// containing the `data` property
  /// </summary>
  public object Data { get; set; }

  /// <summary>
  /// Constructor method which creates and formats the success response
  /// </summary>
  /// <param name="data">The dynamic object</param>
  /// <param name="excludeNullValues">True will remove properties with null values, false will include</param>
  public SuccessResponse(object? data = null, bool excludeNullValues = false) {
    JsonSerializerOptions options = new(JsonSerializerOptions.Web);

    if (excludeNullValues)
      options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    Data = JsonSerializer.SerializeToDocument(data ?? new { }, options);
  }
}