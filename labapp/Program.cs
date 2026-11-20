using labapp.Filters;
using labapp.Infrastructure;

namespace labapp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(o => o.Filters.Add<GlobalExceptionFilter>());

        builder.Services.AddSingleton<RequestStats>();
        builder.Services.AddScoped<LogActionFilter>();

        var app = builder.Build();

        app.UseMiddleware<RequestTimingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/Home/HttpStatus", "?code={0}");

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllerRoute("archive", "archive/{year:int}/{month:int:range(1,12)}",
            new { controller = "Book", action = "Archive" });

        app.MapControllerRoute("books-by-year", "books/year/{year:int:min(1450)}",
            new { controller = "Book", action = "ByYear" });

        app.MapControllerRoute("books-by-isbn", "books/isbn/{isbn:regex(^\\d{{13}}$)}",
            new { controller = "Book", action = "ByIsbn" });

        app.MapControllerRoute("docs", "docs/{*path}",
            new { controller = "Home", action = "Docs" });

        app.MapControllerRoute("admin-area", "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}