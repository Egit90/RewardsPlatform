namespace RewardsService.Models;

public sealed class RewardTransaction
{
    public Guid Id { get; set; }
    public required string CustomerId { get; set; }
    public int Points { get; set; }
    public decimal PurchaseAmount { get; set; }
    public DateTime TimeStamp { get; set; }

    public static RewardTransaction FromAwardRequestAndAccount(AwardRequest request , RewardAccount account) => new()
    {
        Id = Guid.NewGuid(),
        CustomerId = request.CustomerId,
        PurchaseAmount = request.PurchaseAmount,
        Points = account.PointsBalance,
        TimeStamp = DateTime.UtcNow
    };
}