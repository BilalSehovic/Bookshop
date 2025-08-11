namespace DataAccessLayer.Models;

public class Sale : BaseEntity
{
    public Guid BookId { get; set; }
    public DateTime SaleDate { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalPrice { get; set; }
    public string? CustomerName { get; set; }
    public Book Book { get; set; } = null!;
}
