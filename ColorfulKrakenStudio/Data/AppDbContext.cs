using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ColorfulKrakenStudio.Data;

public class AppDbContext : IdentityDbContext<Customer>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Tutorial> Tutorials { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<Microsoft.AspNetCore.Identity.IdentityPasskeyData>();
        modelBuilder.Ignore<Microsoft.AspNetCore.Identity.IdentityUserPasskey<string>>();

        modelBuilder.Entity<Tutorial>().HasData(
            new Tutorial { Id = 1, Title = "Feral Tyranid", Author = "Mamikon", Duration = 45, Price = 0, IsFree = true },
            new Tutorial { Id = 2, Title = "Imperial Fist", Author = "David Arroba", Duration = 60, Price = 9.99m, IsFree = false },
            new Tutorial { Id = 3, Title = "Colorful Wizard", Author = "Kaha Gorska", Duration = 90, Price = 9.99m, IsFree = false },
            new Tutorial { Id = 4, Title = "Classic Space Marine", Author = "David Arroba", Duration = 30, Price = 0, IsFree = true },
            new Tutorial { Id = 5, Title = "Speedpaint Acrylics", Author = "Mamikon", Duration = 75, Price = 9.99m, IsFree = false },
            new Tutorial { Id = 6, Title = "Oil Painting Technique", Author = "Kaha Gorska", Duration = 120, Price = 14.99m, IsFree = false }
        );
    }
}