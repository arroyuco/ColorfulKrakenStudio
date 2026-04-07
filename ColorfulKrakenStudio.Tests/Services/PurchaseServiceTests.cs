using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;
using ColorfulKrakenStudio.Services;
using ColorfulKrakenStudio.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ColorfulKrakenStudio.Tests.Services;

public class PurchaseServiceTests
{
    [Fact]
    public async Task HasAccessAsync_ReturnsTrue_WhenPurchaseExists()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Purchases.Add(new Purchase
            {
                Id = 1,
                UserId = "user1",
                TutorialId = 1,
                AmountPaid = 9.99m,
                PurchasedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        var service = new PurchaseService(DbContextHelper.CreateFactory(dbName));

        var result = await service.HasAccessAsync("user1", 1);

        Assert.True(result);
    }

    [Fact]
    public async Task HasAccessAsync_ReturnsFalse_WhenPurchaseDoesNotExist()
    {
        var service = new PurchaseService(DbContextHelper.CreateFactory(Guid.NewGuid().ToString()));

        var result = await service.HasAccessAsync("user1", 999);

        Assert.False(result);
    }

    [Fact]
    public async Task HasAccessAsync_ReturnsFalse_WhenPurchaseBelongsToAnotherUser()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Purchases.Add(new Purchase
            {
                Id = 1,
                UserId = "user2",
                TutorialId = 1,
                AmountPaid = 9.99m,
                PurchasedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        var service = new PurchaseService(DbContextHelper.CreateFactory(dbName));

        var result = await service.HasAccessAsync("user1", 1);

        Assert.False(result);
    }
    [Fact]
    public async Task RegisterPurchaseAsync_CreatesPurchase_WhenNotExists()
    {
        var dbName = Guid.NewGuid().ToString();
        var service = new PurchaseService(DbContextHelper.CreateFactory(dbName));

        await service.RegisterPurchaseAsync("user1", 1, 9.99m);

        using var db = DbContextHelper.CreateInMemoryContext(dbName);
        var purchase = await db.Purchases.FirstOrDefaultAsync();

        Assert.NotNull(purchase);
        Assert.Equal("user1", purchase.UserId);
        Assert.Equal(1, purchase.TutorialId);
        Assert.Equal(9.99m, purchase.AmountPaid);
    }
    [Fact]
    public async Task RegisterPurchaseAsync_DoesNotDuplicate_WhenAlreadyExists()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Purchases.Add(new Purchase
            {
                Id = 1,
                UserId = "user1",
                TutorialId = 1,
                AmountPaid = 9.99m,
                PurchasedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        var service = new PurchaseService(DbContextHelper.CreateFactory(dbName));

        await service.RegisterPurchaseAsync("user1", 1, 9.99m);

        using var db2 = DbContextHelper.CreateInMemoryContext(dbName);
        var count = await db2.Purchases.CountAsync();
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task GetPurchasedTutorialIdsAsync_ReturnsOnlyIdsForCorrectUser()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Purchases.AddRange(
                new Purchase { Id = 1, UserId = "user1", TutorialId = 1, AmountPaid = 9.99m, PurchasedAt = DateTime.UtcNow },
                new Purchase { Id = 2, UserId = "user1", TutorialId = 3, AmountPaid = 9.99m, PurchasedAt = DateTime.UtcNow },
                new Purchase { Id = 3, UserId = "user2", TutorialId = 2, AmountPaid = 9.99m, PurchasedAt = DateTime.UtcNow }
            );
            await db.SaveChangesAsync();
        }

        var service = new PurchaseService(DbContextHelper.CreateFactory(dbName));

        var result = await service.GetPurchasedTutorialIdsAsync("user1");

        Assert.Equal(2, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(3, result);
        Assert.DoesNotContain(2, result);
    }
}