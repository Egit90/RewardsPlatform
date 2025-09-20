namespace IdentityService.Models;

public record VerifyRequest(string SessionId, string Otp);