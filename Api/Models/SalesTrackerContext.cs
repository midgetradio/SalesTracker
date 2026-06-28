using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public class SalesTrackerContext : DbContext
    {
        public SalesTrackerContext(DbContextOptions<SalesTrackerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SaleType> SaleTypes { get; set; } = null!;
        public virtual DbSet<Edition> Editions { get; set; } = null!;
        public virtual DbSet<PageHit> PageHits { get; set; } = null!;
    }
}

