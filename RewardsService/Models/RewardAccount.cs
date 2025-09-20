namespace RewardsService.Models;

public sealed class RewardAccount
{
    public Guid Id { get; set; }
    public required string CustomerId { get; set; }
    public int PointsBalance { get; set; }
    public DateTime LastUpdate { get; set; }

    public static RewardAccount FromRequest(AwardRequest request) => new()
    {
        Id = Guid.NewGuid(),
        CustomerId = request.CustomerId,
        PointsBalance = 0,
    };
}