namespace Movies.Contracts.Responses.Identity;

[Description("This class formats the successful user details response")]
public struct UserMeResponse {
  [Required, JsonPropertyName("id"), Description("The unique identifier")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required long Id { get; set; }

  [Required, JsonPropertyName("fullname"), Description("The user's first and last name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Fullname { get; init; }

  [Required, JsonPropertyName("firstName"), Description("The user's first name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string FirstName { get; init; }

  [Required, JsonPropertyName("lastName"), Description("The user's last name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string LastName { get; init; }

  [Required, JsonPropertyName("email"), Description("The email address")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Email { get; init; }

  [Required, JsonPropertyName("role"), Description("The role name")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Role { get; init; }

  [Required, JsonPropertyName("status"), Description("Account status")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required string Status { get; set; }

  [Required, JsonPropertyName("createdAt"), Description("Account registered date")]
  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public required DateTime? CreatedAt { get; set; }
}