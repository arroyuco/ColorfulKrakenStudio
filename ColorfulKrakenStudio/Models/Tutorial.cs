namespace ColorfulKrakenStudio.Models;

public class Tutorial
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public bool IsFree { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}