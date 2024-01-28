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
            var result = _context.ApiModel
                                 .FromSqlRaw<ApiModel>("exec dbo.usp_api")
                                 .ToList();

            return Ok(Json(result));
        }
    }
}
