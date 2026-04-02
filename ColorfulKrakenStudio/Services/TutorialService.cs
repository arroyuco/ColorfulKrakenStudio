using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class TutorialService
{
    private readonly IDbContextFactory<AppDbContext> _db;

    public TutorialService(IDbContextFactory<AppDbContext> db)
    {
        _db = db;
    }

    public async Task<List<Tutorial>> GetAllAsync()
    {
        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Tutorials
            .Where(t => t.IsPublished)
            .OrderBy(t => t.Id)
            .ToListAsync();
    }

    public async Task<Tutorial?> GetByIdAsync(int id)
    {
        await using var ctx = await _db.CreateDbContextAsync();
        return await ctx.Tutorials.FindAsync(id);
    }
}