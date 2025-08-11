using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
