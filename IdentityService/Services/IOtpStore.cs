namespace IdentityService.Services;

public interface IOtpStore
{
    Task SaveAsync(string sessionId, string otp, TimeSpan ttl);
    Task<string?> GetAsync(string sessionId);
    Task RemoveAsync(string sessionId);
}