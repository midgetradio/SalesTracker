using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Data;
using SalesTracker.Models;
using SalesTracker.ViewModels;
using System.Diagnostics;

namespace SalesTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SalesTrackerDBContext _context;

        public HomeController(ILogger<HomeController> logger, SalesTrackerDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string date, int index)
        {
            var model = new HomeVM();
            model.Dates = _context.Editions.OrderBy(o => o.LastUpdated).Select(s => s.LastUpdated.Date).Distinct().ToList();
            

            if(String.IsNullOrEmpty(date) || date == "All Time")
            {
                model.Editions = _context.Editions.Include(i => i.SaleType).OrderBy(o => o.Title).ToList();
                model.SelectedIndex = 0;
            }
            else
            {
                var selectedDate = DateTime.Parse(date);
                model.Editions = _context.Editions.Include(i => i.SaleType).Where(w => w.LastUpdated >= selectedDate).OrderBy(o => o.Title).ToList();
                model.SelectedIndex = index;
            }

            return View(model);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}