using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class EditionsService
    {
        private readonly SalesTrackerContext _context;

        public EditionsService(SalesTrackerContext context)
        {
            _context = context;
        }

        public async Task<string[]> GetLastTenDates()
        {
            var dates = await _context.Editions.Where(w => w.IsDeleted == false)
                                                 .Select(s => s.LastUpdated.Date)
                                                 .Distinct()
                                                 .OrderByDescending(o => o.Date)
                                                 .Take(10)
                                                 .ToListAsync();

            return dates.Select(s => s.ToString("MM/dd/yyyy")).ToArray();
        }

        public async Task<List<Edition>> GetEditionsByDateAsync(DateTime date)
        {
            Console.WriteLine($"Fetching editions for date: {date.ToString("MM/dd/yyyy")}");
            return await _context.Editions.Where(w => w.LastUpdated.Date >= date.Date && w.IsDeleted == false)
                                            .Include(i => i.SaleType)
                                            .OrderBy(o => o.Title)
                                            .ToListAsync();
        }

        public async Task<List<Edition>> GetEditionsAsync()
        {
            return await _context.Editions.Where(w => w.IsDeleted == false)
                                            .Include(i => i.SaleType)
                                            .OrderBy(o => o.Title)
                                            .ToListAsync();
        }

        public async Task<SaleType[]> GetSaleTypesAsync()
        {
            var saleTypes = await _context.SaleTypes.Where(w => w.IsActive)
                                           .OrderBy(o => o.Type)
                                           .ToArrayAsync();

            return saleTypes;
        }
    }
}