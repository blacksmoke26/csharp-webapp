namespace Movies.Contracts.Responses.Identity;

[SwaggerSchema("This class formats the successful user details response",
  ReadOnly = true)]
public struct UserMeResponse {
  [SwaggerSchema("The unique identifier")]
  public required long Id { get; set; }

  [SwaggerSchema("The user's first and last name", Nullable = false)]
  public required string Fullname { get; init; }

  [SwaggerSchema("The user's first name", Nullable = false)]
  public required string FirstName { get; init; }

  [SwaggerSchema("The user's last name", Nullable = false)]
  public required string LastName { get; init; }

  [SwaggerSchema("The email address", Format = "email", Nullable = false)]
  public required string Email { get; init; }

  [SwaggerSchema("The role name", Nullable = false)]
  public required string Role { get; init; }

  [SwaggerSchema("Account status", Nullable = false)]
  public required string Status { get; set; }

  [SwaggerSchema("Account registered date")]
  public required DateTime CreatedAt { get; set; }
}