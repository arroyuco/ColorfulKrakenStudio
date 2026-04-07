using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;
using ColorfulKrakenStudio.Services;
using ColorfulKrakenStudio.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ColorfulKrakenStudio.Tests.Services;

public class TutorialServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsOnlyPublishedTutorials()
    {
        var dbName = Guid.NewGuid().ToString();

        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Tutorials.AddRange(
                new Tutorial { Id = 1, Title = "Published", Author = "Author", Duration = 30, Price = 0, IsFree = true, IsPublished = true },
                new Tutorial { Id = 2, Title = "Unpublished", Author = "Author", Duration = 30, Price = 9.99m, IsFree = false, IsPublished = false }
            );
            await db.SaveChangesAsync();
        }

        var service = new TutorialService(DbContextHelper.CreateFactory(dbName));

        var result = await service.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("Published", result[0].Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTutorials()
    {
        var service = new TutorialService(DbContextHelper.CreateFactory(Guid.NewGuid().ToString()));

        var result = await service.GetAllAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsTutorial_WhenExists()
    {
        var dbName = Guid.NewGuid().ToString();
        using (var db = DbContextHelper.CreateInMemoryContext(dbName))
        {
            db.Tutorials.Add(new Tutorial
            {
                Id = 1,
                Title = "Test",
                Author = "Author",
                Duration = 30,
                Price = 0,
                IsFree = true,
                IsPublished = true
            });
            await db.SaveChangesAsync();
        }

        var service = new TutorialService(DbContextHelper.CreateFactory(dbName));

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var service = new TutorialService(DbContextHelper.CreateFactory(Guid.NewGuid().ToString()));

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }
}