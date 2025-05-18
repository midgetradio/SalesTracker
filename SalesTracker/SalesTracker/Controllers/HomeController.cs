using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Data;
using SalesTracker.Models;
using SalesTracker.Utility;
using SalesTracker.ViewModels;
using System.Diagnostics;

namespace SalesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly SalesTrackerDBContext _context;
        private readonly PageHitsTracker _tracker;

        public HomeController(SalesTrackerDBContext context, PageHitsTracker tracker)
        {
            _context = context;
            _tracker = tracker;
        }

        public IActionResult Index(string date, int index)
        {
            var model = new HomeVM();
            model.Dates = _context.Editions.Where(w => w.IsDeleted == false)
                                           .Select(s => s.LastUpdated.Date)
                                           .Distinct()
                                           .OrderByDescending(o => o.Date)
                                           .Take(10)
                                           .ToList();
            model.Dates = model.Dates.OrderBy(o => o.Date).ToList();
            model.SaleTypes = _context.SaleTypes.Where(w => w.IsActive).ToList();

            if (String.IsNullOrEmpty(date) || date == "All Time")
            {
                model.Editions = _context.Editions
                                         .Include(i => i.SaleType)
                                         .OrderBy(o => o.Title)
                                         .Where(w => w.IsDeleted == false)
                                         .ToList();
                model.SelectedIndex = 0;
            }
            else
            {
                var selectedDate = DateTime.Parse(date);
                model.Editions = _context.Editions
                                         .Include(i => i.SaleType)
                                         .Where(w => w.LastUpdated >= selectedDate && w.IsDeleted == false)
                                         .OrderBy(o => o.Title)
                                         .ToList();
                model.SelectedIndex = index;
            }

            _tracker.AddPageHit(_context);

            return View(model);
        }

        public IActionResult Removed()
        {
            var model = _context.Editions
                                .Include(i => i.SaleType)
                                .Where(w => w.IsDeleted)
                                .OrderByDescending(o => o.LastUpdated)
                                .ThenBy(to => to.Title)
                                .ToList();

            return View(model);
        }

        public IActionResult Stats()
        {
            var model = _context.PageHits.OrderByDescending(o => o.Date).ToList();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}