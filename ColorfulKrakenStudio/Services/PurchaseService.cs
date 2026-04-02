using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class PurchaseService
{
    private readonly IDbContextFactory<AppDbContext> _db;

    public PurchaseService(IDbContextFactory<AppDbContext> db)
    {
        _db = db;
    }

    public async Task<bool> HasAccessAsync(string customerId, int tutorialId)
    {
        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Purchases
            .AnyAsync(p => p.UserId == customerId && p.TutorialId == tutorialId);
    }


    public async Task RegisterPurchaseAsync(string customerId, int tutorialId, decimal amountPaid)
    {
        await using var ctx = await _db.CreateDbContextAsync();
        var exists = await HasAccessAsync(customerId, tutorialId);
        if (exists) return;

        ctx.Purchases.Add(new Purchase
        {
            UserId = customerId,
            TutorialId = tutorialId,
            AmountPaid = amountPaid,
            PurchasedAt = DateTime.UtcNow
        });

        await ctx.SaveChangesAsync();
    }

    public async Task<List<int>> GetPurchasedTutorialIdsAsync(string customerId)
    {

        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Purchases
            .Where(p => p.UserId == customerId)
            .Select(p => p.TutorialId)
            .ToListAsync();
    }

    public async Task<List<Tutorial>> GetPurchasedTutorialsAsync(string customerId)
    {
        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Purchases
            .Where(p => p.UserId == customerId)
            .Include(p => p.Tutorial)
            .OrderByDescending(p => p.PurchasedAt)
            .Select(p => p.Tutorial)
            .ToListAsync();
    }

    public async Task<List<Purchase>> GetPurchaseHistoryAsync(string customerId)
    {
        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Purchases
            .Where(p => p.UserId == customerId)
            .Include(p => p.Tutorial)
            .OrderByDescending(p => p.PurchasedAt)
            .ToListAsync();
    }
}