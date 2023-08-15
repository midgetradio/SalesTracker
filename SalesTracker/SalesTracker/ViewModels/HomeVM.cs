using Microsoft.AspNetCore.Mvc.Rendering;
using SalesTracker.Models;

namespace SalesTracker.ViewModels
{
    public class HomeVM
    {
        public int SelectedIndex { get; set; }
        public List<DateTime> Dates { get; set; } = null!;
        public List<Edition> Editions { get; set; } = null!;

    }
}
