namespace DataAccessLayer.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
