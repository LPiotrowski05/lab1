using labapp.Infrastructure;
using labapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace labapp.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly RequestStats _stats;
    public DashboardController(RequestStats stats) => _stats = stats;

    public IActionResult Index()
    {
        var vm = new DashboardViewModel
        {
            Total = _stats.Total,
            TopPaths = _stats.Paths.OrderByDescending(p => p.Value).Take(5).ToList()
        };
        return View(vm);
    }
}