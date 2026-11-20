using System.Diagnostics;
using labapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace labapp.Controllers;

public class HomeController : Controller
{
    private readonly IActionDescriptorCollectionProvider _provider;

    public HomeController(IActionDescriptorCollectionProvider provider) => _provider = provider;

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    public IActionResult HttpStatus(int code)
    {
        Response.StatusCode = code;
        ViewBag.Code = code;
        ViewBag.Description = code switch
        {
            404 => "Nie znaleziono strony",
            403 => "Brak dostępu",
            405 => "Niedozwolona metoda",
            500 => "Błąd serwera",
            _ => "Wystąpił błąd"
        };
        return View();
    }

    public IActionResult Docs(string? path)
    {
        ViewBag.Path = path;
        return View();
    }

    public IActionResult Routes()
    {
        var rows = _provider.ActionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .Select(a =>
            {
                a.RouteValues.TryGetValue("area", out var area);
                var url = a.AttributeRouteInfo?.Template is { } t
                    ? "/" + t
                    : "/" + (string.IsNullOrEmpty(area) ? "" : area + "/") + a.ControllerName + "/" + a.ActionName;

                var methods = a.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                    .SelectMany(c => c.HttpMethods).Distinct().ToList();

                return new RouteRow(
                    url,
                    $"{a.ControllerName}.{a.ActionName}",
                    methods is { Count: > 0 } ? string.Join(", ", methods) : "ANY");
            })
            .OrderBy(r => r.Url)
            .ToList();

        return View(rows);
    }

    public IActionResult Boom() => throw new InvalidOperationException("Celowy wyjątek");
}