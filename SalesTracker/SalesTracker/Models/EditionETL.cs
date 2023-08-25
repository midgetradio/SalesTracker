using System.ComponentModel.DataAnnotations;

namespace SalesTracker.Models
{
    public class EditionETL
    {
        [MaxLength(250)]
        public string Title { get; set; } = String.Empty;
        [MaxLength(1000)]
        public string URL { get; set; } = String.Empty;
        [MaxLength(20)]
        public string Price { get; set; } = String.Empty;
        [MaxLength(20)]
        public string Discount { get; set; } = String.Empty;

        public string SaleType { get; set; } = String.Empty;
    }
}
