namespace ColorfulKrakenStudio.Models;

public class Purchase
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int TutorialId { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime PurchasedAt { get; set; }
    public Tutorial Tutorial { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}