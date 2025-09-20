using StackExchange.Redis;

namespace IdentityService.Services;

public class RedisOtpStore(IConnectionMultiplexer redis) : IOtpStore
{
    private readonly IDatabase _db = redis.GetDatabase();

    public Task SaveAsync(string sessionId, string otp, TimeSpan ttl) => _db.StringSetAsync(sessionId, otp, ttl);

    public async Task<string?> GetAsync(string sessionId)
    {
        var value = await _db.StringGetAsync(sessionId);
        return value.HasValue ? value.ToString() : null;
    }

    public Task RemoveAsync(string sessionId) => _db.KeyDeleteAsync(sessionId);
}