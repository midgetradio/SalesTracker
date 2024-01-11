using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Data;
using SalesTracker.Models;
using System.Text.Json;

namespace SalesTracker.Controllers
{
    public class ApiController : Controller
    {
        private SalesTrackerDBContext _context;

        public ApiController(SalesTrackerDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetLatest()
        {
            var date = DateTime.Now;

            var result = _context.Editions
                                 .Include(i => i.SaleType)
                                 .Where(w => w.LastUpdated.Date == date.Date && w.IsDeleted == false)
                                 .Select(s => new { s.Title, url = ("https://www.instocktrades.com" + s.URL), s.Price, s.Discount, s.SaleType.Type })
                                 .OrderBy(o => o.Title)
                                 .ToList();

            return Ok(Json(result));
        }
    }
}
