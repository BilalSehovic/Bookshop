using System;
using System.ComponentModel.DataAnnotations;

namespace BookshopWpf.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Isbn { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
