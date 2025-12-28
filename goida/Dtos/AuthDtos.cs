namespace goida.Dtos;

public record RegisterRequest(string Email, string Password, string DisplayName);
public record LoginRequest(string Email, string Password);
public record LoginResponse(string Token, DateTimeOffset ExpiresAt);
