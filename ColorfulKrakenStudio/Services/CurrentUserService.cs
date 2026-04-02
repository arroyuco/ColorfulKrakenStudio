using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ColorfulKrakenStudio.Data;
using ColorfulKrakenStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorfulKrakenStudio.Services;

public class CurrentUserService
{
    private readonly IDbContextFactory<AppDbContext> _db;
    private Customer? _cachedCustomer;

    public CurrentUserService(IDbContextFactory<AppDbContext> db)
    {
        _db = db;
    }

    public async Task<Customer?> GetCustomerAsync(ClaimsPrincipal user)
    {
        if (_cachedCustomer != null)
            return _cachedCustomer;

        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(id))
            return null;

        await using var ctx = await _db.CreateDbContextAsync();
        _cachedCustomer = await ctx.Users.FirstOrDefaultAsync(u => u.Id == id);
        return _cachedCustomer;
    }

    public void Invalidate() => _cachedCustomer = null;

    public bool IsAuthenticated => _cachedCustomer != null;
    public string FirstName => _cachedCustomer?.FirstName ?? string.Empty;
    public string LastName => _cachedCustomer?.LastName ?? string.Empty;
    public string Email => _cachedCustomer?.Email ?? string.Empty;
    public string Id => _cachedCustomer?.Id ?? string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
}