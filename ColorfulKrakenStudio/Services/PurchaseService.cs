using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class PurchaseService
{
    private readonly AppDbContext _db;

    public PurchaseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HasAccessAsync(string customerId, int tutorialId)
        => await _db.Purchases
            .AnyAsync(p => p.UserId == customerId && p.TutorialId == tutorialId);

    public async Task RegisterPurchaseAsync(string customerId, int tutorialId, decimal amountPaid)
    {
        var exists = await HasAccessAsync(customerId, tutorialId);
        if (exists) return;

        _db.Purchases.Add(new Purchase
        {
            UserId = customerId,
            TutorialId = tutorialId,
            AmountPaid = amountPaid,
            PurchasedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }

    public async Task<List<int>> GetPurchasedTutorialIdsAsync(string customerId)
        => await _db.Purchases
            .Where(p => p.UserId == customerId)
            .Select(p => p.TutorialId)
            .ToListAsync();
}