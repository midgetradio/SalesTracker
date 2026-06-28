using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class PageHit
    {
        [Key]
        public int Id { get; set; }

        public int Hits { get; set; }
        public DateTime Date { get; set; }
    }
}