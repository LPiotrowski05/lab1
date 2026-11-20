using labapp.Models;
using Microsoft.AspNetCore.Mvc;

namespace labapp.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthorsController : Controller
{
    public IActionResult Index() => View(AuthorsMemory.Authors);

    [HttpGet]
    public IActionResult Create() => View(new AuthorFormViewModel());

    [HttpPost]
    public IActionResult Create(AuthorFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        AuthorsMemory.Authors.Add(new Author
        {
            Id = AuthorsMemory.Authors.Max(a => a.Id) + 1,
            Name = vm.Name
        });

        TempData["Success"] = $"Dodano autora „{vm.Name}”.";
        return RedirectToAction(nameof(Index));
    }
}