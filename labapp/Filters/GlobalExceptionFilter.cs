using System.Diagnostics;
using labapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace labapp.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _log;
    private readonly IModelMetadataProvider _meta;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> log, IModelMetadataProvider meta)
    {
        _log = log;
        _meta = meta;
    }

    public void OnException(ExceptionContext context)
    {
        _log.LogError(context.Exception, "Nieobsłużony wyjątek w {Path}", context.HttpContext.Request.Path);

        if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Status = 500,
                Title = "Wystąpił błąd serwera"
            })
            { StatusCode = 500 };
        }
        else
        {
            context.Result = new ViewResult
            {
                ViewName = "Error",
                StatusCode = 500,
                ViewData = new ViewDataDictionary(_meta, context.ModelState)
                {
                    Model = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                    }
                }
            };
        }

        context.ExceptionHandled = true;
    }
}