using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class PageHitService
    {
        private readonly SalesTrackerContext _context;

        public PageHitService(SalesTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<PageHit>> GetPageHitsLastThirty()
        {
            var hits = await _context.PageHits.OrderByDescending(o => o.Date)
                                            .Take(30)
                                            .ToListAsync();

            return hits;
        }
    }
}