using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Sale : BaseEntity
{
    public Guid? BookId { get; set; }

    [Required]
    public DateTime SaleDate { get; set; } = DateTime.Now;

    public string BookDescription { get; set; }

    public int Quantity { get; set; }

    [Required]
    public double UnitPrice { get; set; }

    public double TotalPrice { get; set; }

    public string? CustomerName { get; set; }

    public Book Book { get; set; } = null!;
}
