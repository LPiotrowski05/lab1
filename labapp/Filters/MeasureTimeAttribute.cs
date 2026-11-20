using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace labapp.Filters;

public class MeasureTimeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext c) =>
        c.HttpContext.Items["action-sw"] = Stopwatch.StartNew();

    public override void OnActionExecuted(ActionExecutedContext c)
    {
        if (c.HttpContext.Items["action-sw"] is Stopwatch sw)
            c.HttpContext.Response.Headers["X-Action-Ms"] = sw.ElapsedMilliseconds.ToString();
    }
}