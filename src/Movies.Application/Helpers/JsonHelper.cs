// Licensed to the end users under one or more agreements.
// Copyright (c) 2025 Junaid Atari, and contributors
// Repository:https://github.com/blacksmoke26/csharp-webapp

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Movies.Application.Helpers;

public static class JsonHelper {
  /// <summary>
  /// Converts the JSON string into an object
  /// </summary>
  /// <param name="json">The JSON text</param>
  /// <returns>The JSON object</returns>
  public static TReturn? FromJsonString<TReturn>(string json) where TReturn : class {
    try {
      return JsonConvert.DeserializeObject<TReturn>(json);
    }
    catch {
      return null;
    }
  }

  /// <summary>
  /// Converts the object into a JSON text representation
  /// </summary>
  /// <param name="data">The object to convert</param>
  /// <param name="formatting">JSON text Indentation</param>
  /// <returns>The JSON string</returns>
  public static string ToJsonString(object data, Formatting formatting = Formatting.None) {
    return JsonConvert.SerializeObject(data, formatting, new JsonSerializerSettings() {
      ContractResolver = new CamelCasePropertyNamesContractResolver()
    });
  }
}