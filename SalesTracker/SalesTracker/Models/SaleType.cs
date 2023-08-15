using System.ComponentModel.DataAnnotations;

namespace SalesTracker.Models
{
    public class SaleType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = String.Empty;
        [MaxLength(500)]
        public string URL { get; set; } = String.Empty;
    }
}
