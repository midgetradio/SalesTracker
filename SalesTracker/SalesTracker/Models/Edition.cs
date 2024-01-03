using System.ComponentModel.DataAnnotations;

namespace SalesTracker.Models
{
    public class Edition
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; } = String.Empty;
        [MaxLength(1000)]
        public string URL { get; set; } = String.Empty;
        [MaxLength(20)]
        public string Price { get; set; } = String.Empty;
        [MaxLength(20)]
        public string Discount { get; set; } = String.Empty;

        public DateTime LastUpdated { get; set; }

        public virtual SaleType SaleType { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public string DataId { get; set; } = String.Empty;
    }
}
