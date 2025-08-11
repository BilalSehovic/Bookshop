using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopWpf.Models
{
    public class Sale
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BookId { get; set; }

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public int Quantity { get; set; } = 1;
    }
}
