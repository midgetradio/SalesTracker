using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesTracker.Models
{
    [NotMapped]
    public class ApiModel
    {
        public string Title { get; set; } = String.Empty;
        public string URL { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string SalesType { get; set; } = String.Empty;
        public string Discount { get; set; } = String.Empty;
    }
}
