using Microsoft.EntityFrameworkCore;
using ColorfulKrakenStudio.Models;

namespace ColorfulKrakenStudio.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Tutorial> Tutorials { get; set; }
    public DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutorial>().HasData(
            new Tutorial { Id = 1, Title = "Feral Tyranid", Author = "Mamikon", Duration = 45, Plan = "Free", IsFree = true },
            new Tutorial { Id = 2, Title = "Imperial Fist", Author = "David Arroba", Duration = 60, Plan = "Basic", IsFree = false },
            new Tutorial { Id = 3, Title = "Colorful Wizard", Author = "Kaha Gorska", Duration = 90, Plan = "Pro", IsFree = false },
            new Tutorial { Id = 4, Title = "Classic Space Marine", Author = "David Arroba", Duration = 30, Plan = "Free", IsFree = true },
            new Tutorial { Id = 5, Title = "Speedpaint Acrylics", Author = "Mamikon", Duration = 75, Plan = "Pro", IsFree = false },
            new Tutorial { Id = 6, Title = "Oil Painting Technique", Author = "Kaha Gorska", Duration = 120, Plan = "Pro", IsFree = false }
        );
    }
}