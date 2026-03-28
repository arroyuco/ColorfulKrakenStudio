using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Services;

public class TutorialService
{
    private readonly AppDbContext _db;

    public TutorialService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Tutorial>> GetAllAsync()
        => await _db.Tutorials
            .Where(t => t.IsPublished)
            .OrderBy(t => t.Id)
            .ToListAsync();

    public async Task<Tutorial?> GetByIdAsync(int id)
        => await _db.Tutorials.FindAsync(id);
}