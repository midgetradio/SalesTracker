using System.ComponentModel.DataAnnotations;

namespace SalesTracker.Models
{
    public class PageHits
    {
        [Key]
        public int Id { get; set; }

        public int Hits { get; set; }
        public DateTime Date { get; set; }

    }
}
