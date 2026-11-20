using Microsoft.AspNetCore.Mvc.Filters;

namespace labapp.Filters;

public class SecondLogAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext c)
    {
        var log = c.HttpContext.RequestServices.GetRequiredService<ILogger<SecondLogAttribute>>();
        log.LogInformation("DRUGI filtr: START {Action}", c.ActionDescriptor.DisplayName);
    }

    public override void OnActionExecuted(ActionExecutedContext c)
    {
        var log = c.HttpContext.RequestServices.GetRequiredService<ILogger<SecondLogAttribute>>();
        log.LogInformation("DRUGI filtr: KONIEC {Action}", c.ActionDescriptor.DisplayName);
    }
}