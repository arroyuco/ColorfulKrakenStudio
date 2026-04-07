using ColorfulKrakenStudio.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ColorfulKrakenStudio.Tests.Helpers;

public static class DbContextHelper
{
    public static AppDbContext CreateInMemoryContext(string dbName = "")
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(string.IsNullOrEmpty(dbName)
                ? Guid.NewGuid().ToString()
                : dbName)
            .Options;

        return new AppDbContext(options);
    }

    public static IDbContextFactory<AppDbContext> CreateFactory(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var mock = new Moq.Mock<IDbContextFactory<AppDbContext>>();
        mock.Setup(f => f.CreateDbContextAsync(default))
            .ReturnsAsync(() => new AppDbContext(options));

        return mock.Object;
    }
}