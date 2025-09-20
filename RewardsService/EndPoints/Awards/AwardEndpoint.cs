using Microsoft.EntityFrameworkCore;
using RewardsService.Data;
using RewardsService.Models;

namespace RewardsService.EndPoints.Awards;

public static class AwardEndpoint
{
    public static IEndpointRouteBuilder MapAwards(this WebApplication app)
    {
        var group = app.MapGroup("/rewards");
        group.MapPost("/award", Award);
        group.MapGet("/{customerId}", GetBalance);

        return group;
    }

    private static async Task<IResult> GetBalance(string customerId, RewardsDbContext db)
    {
        var account = await db.RewardAccounts
            .FirstOrDefaultAsync(a => a.CustomerId == customerId);

        return account == null
            ? Results.NotFound(new { error = "Account not found" })
            : Results.Ok(new { customerId = account.CustomerId, balance = account.PointsBalance });
    }

    private static async Task<IResult> Award(AwardRequest request, RewardsDbContext db)
    {
        const decimal pointsPerDollar = 1m;
        int points = (int)(request.PurchaseAmount * pointsPerDollar);

        var account = await db.RewardAccounts
            .FirstOrDefaultAsync(a => a.CustomerId == request.CustomerId);

        if (account == null)
        {
            account = RewardAccount.FromRequest(request);
            await db.RewardAccounts.AddAsync(account);
        }

        account.PointsBalance += points;
        account.LastUpdate = DateTime.UtcNow;

        // log transaction
        db.RewardTransactions.Add(RewardTransaction.FromAwardRequestAndAccount(request, account));

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            message = $"Awarded {points} points",
            balance = account.PointsBalance
        });
    }
}