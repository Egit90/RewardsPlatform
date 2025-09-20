using Microsoft.EntityFrameworkCore;
using RewardsService.Models;

namespace RewardsService.Data;

public sealed class RewardsDbContext(DbContextOptions ops) : DbContext(ops)
{
    public DbSet<RewardAccount> RewardAccounts => Set<RewardAccount>();
    public DbSet<RewardTransaction> RewardTransactions => Set<RewardTransaction>();
}